using AutoStopAPI.Models;
using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageAPIController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostMessage([FromBody] Message message) 
        {
            SQLMessage sqlMessage = new SQLMessage();
            int idMessage = sqlMessage.AddMessage(message);
            return Ok(idMessage);
        }

        [HttpGet]
        public IActionResult GetMessage([FromQuery] int idChat) 
        {
            var sqlMessage = new SQLMessage();
            List<Message> messages = sqlMessage.GetMessageById(idChat);
            return Ok(messages);
        }
    }
}
