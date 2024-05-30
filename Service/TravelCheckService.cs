using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoStopAPI.Models.SQL;

public class TravelCheckService : BackgroundService
{
    private readonly ILogger<TravelCheckService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TravelCheckService(ILogger<TravelCheckService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("TravelCheckService running at: {time}", DateTimeOffset.Now);

            using (var scope = _serviceProvider.CreateScope())
            {
                var sqlTravel = scope.ServiceProvider.GetRequiredService<SQLTravelService>();
                var now = DateTime.Now;

                var travelsToUpdate = sqlTravel.GetInactiveTravels(now);

                if (travelsToUpdate.Any())
                {
                    foreach (var travel in travelsToUpdate)
                    {
                        travel.isActive = false;
                        sqlTravel.UpdateTravel(travel);
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); // Запускаем проверку каждые 10 минут
        }
    }
}
