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
        public IActionResult SearchTravel([FromBody]PassengerSearch passenger)
        {
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