using AutoStopAPI.Models;
using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostUser([FromBody] User user)
        {
            SQLUser sqlUser = new SQLUser();
            bool result = sqlUser.UpdateUser(user);
            if (result == false) 
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
