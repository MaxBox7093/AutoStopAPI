using AutoStopAPI.Models.SQL;
using AutoStopAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [Route("api/Travel")]
    [ApiController]
    public class TravelAPIController : Controller
    {
        [HttpPost]
        public IActionResult PostTravel([FromBody] Travel travel)
        {
            SQLTravel sqlTravel = new SQLTravel();
            int? result = sqlTravel.CreateTravel(travel);

            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetTravel([FromBody] Travel travel) 
        {
            SQLTravel sqlTravel = new SQLTravel();
            List<Travel> travels = sqlTravel.GetTravelsByDriverPhone(travel.phoneDriver!);

            if (travels == null || travels.Count == 0)
            {
                return NotFound(new { message = "No travels found for the given driver phone number" });
            }
            return Ok(travels);
        }

        [HttpDelete]
        public IActionResult DeleteTravel(int idTravel)
        {
            SQLTravel sqlTravel = new SQLTravel();
            bool result = sqlTravel.DeleteTravel(idTravel);

            if (!result)
            {
                return NotFound(new { message = "Travel not found or could not be deleted" });
            }
            return Ok(new { message = "Travel deleted successfully" });
        }
    }
}
