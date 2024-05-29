using AutoStopAPI.Models.SQL;
using AutoStopAPI.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/passenger")]
[ApiController]
public class PassengerAPIController : ControllerBase
{
    [HttpPost]
    public IActionResult PostPassenger([FromBody] Passenger passenger)
    {
        SQLPassenger sqlPassenger = new SQLPassenger();
        var result = sqlPassenger.AddPassenger(passenger);

        switch (result)
        {
            case AddResult.Added:
                return Ok("Passenger added to travel");
            case AddResult.AlreadyExists:
                return Conflict("Passenger already exists in travel");
            case AddResult.Error:
                return BadRequest("Error! Passenger not added to travel");
            default:
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
        }
    }

    [HttpDelete]
    public IActionResult DeletePassenger([FromBody] Passenger passenger)
    {
        SQLPassenger sqlPassenger = new SQLPassenger();
        bool result = sqlPassenger.RemovePassenger(passenger);

        if (result)
        {
            return Ok("Passenger removed from travel");
        }
        else
        {
            return NotFound("Passenger not found in travel or error occurred");
        }
    }
}
