using AutoStopAPI.Models;
using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginAPIController : Controller
    {
        [HttpPost]
        public IActionResult PostUser([FromBody] Login login)
        {
            SQLLogin sqlLogin = new SQLLogin();
            var result = sqlLogin.Login(login);
            return Ok(result);
        }
    }
}
