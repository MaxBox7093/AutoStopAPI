using AutoStopAPI.Models;
using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoStopAPI.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatAPIController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetChats([FromQuery] string phone)
        {
            SQLChat sqlchat = new SQLChat();
            List<Chat> chats = await sqlchat.GetChatsAsync(phone);
            if (chats == null)
                return BadRequest("Error. Chat not found!");
            return Ok(chats);
        }

        [HttpPost]
        public async Task<IActionResult> PostChat([FromBody] Chat chat)
        {
            SQLChat sqlChat = new SQLChat();
            chat = await sqlChat.CreateChatAsync(chat);
            if (chat == null)
                return BadRequest("Error. Chat don't create!");
            return Ok(chat);
        }

        [HttpDelete]
        public IActionResult DeleteChat([FromBody] Chat chat)
        {
            return Ok();
        }
    }
}
