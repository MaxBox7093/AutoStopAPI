using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [Route("api/GetName")]
    [ApiController]
    public class GetNameToPhone : ControllerBase
    {
        [HttpGet]
        public IActionResult GetName([FromQuery] string phone) 
        {
            SQLGetName getname = new SQLGetName();
            string name = getname.GetName(phone);
            return Ok(name);
        }
    }
}
