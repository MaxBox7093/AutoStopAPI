using AutoStopAPI.Models;
using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoStopAPI.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageAPIController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] Message message)
        {
            SQLMessage sqlMessage = new SQLMessage();
            int idMessage = await sqlMessage.AddMessageAsync(message);
            return Ok(idMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessage([FromQuery] int idChat)
        {
            var sqlMessage = new SQLMessage();
            List<Message> messages = await sqlMessage.GetMessageByIdAsync(idChat);
            return Ok(messages);
        }
    }
}
