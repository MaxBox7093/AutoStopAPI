using AutoStopAPI.Models.SQL;
using AutoStopAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AutoStopAPI.Controllers
{
    [Route("api/searchTravel")]
    [ApiController]
    public class TravelSearchPassengerAPIController : Controller
    {
        [HttpGet]
        public IActionResult SearchTravel([FromQuery] string startCity, string endCity, int numberPassenger, DateOnly date)
        {
            PassengerSearch passenger = new PassengerSearch();
            passenger.startCity = startCity;
            passenger.endCity = endCity;
            passenger.numberPassenger = numberPassenger;
            passenger.date = date;
            try
            {
                SQLSearchTravelPassenger search = new SQLSearchTravelPassenger();
                List<Travel> result = search.SearchTravel(passenger);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Console.WriteLine("Ошибка при поиске поездки: " + ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}