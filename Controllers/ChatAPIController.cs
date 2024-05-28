using AutoStopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatAPIController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetChats([FromQuery]string phone) 
        {
            return Ok();
        }
        
        [HttpPost]
        public IActionResult PostChat([FromBody] Chat chat) 
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteChat([FromBody] Chat chat) 
        { 
            return Ok();
        }
    }
}
