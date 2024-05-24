using AutoStopAPI.Models;
using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [ApiController]
    [Route("api/registration")]
    public class RegistrationAPIController : Controller
    {
        [HttpPost]
        public IActionResult PostRegistration([FromBody] Registration registration)
        {
            SQLRegistration SQLReg = new SQLRegistration();
            SQLReg.RegistrationUser(registration);
            return Ok();
        }
    }
}
