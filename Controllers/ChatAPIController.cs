using AutoStopAPI.Models;
using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AutoStopAPI.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatAPIController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetChats([FromQuery]string phone) 
        {
            SQLChat sqlchat = new SQLChat();
            List<Chat> chats = sqlchat.GetChats(phone);
            if (chats == null)
                return BadRequest("Error. Chat not found!");
            return Ok(chats);
        }
        
        [HttpPost]
        public IActionResult PostChat([FromBody] Chat chat) 
        {
            SQLChat sqlChat = new SQLChat();
            chat = sqlChat.CreateChat(chat);
            if (chat == null)
                return BadRequest("Error. Chat dont create!");
            return Ok(chat);
        }

        [HttpDelete]
        public IActionResult DeleteChat([FromBody] Chat chat) 
        { 
            return Ok();
        }
    }
}
