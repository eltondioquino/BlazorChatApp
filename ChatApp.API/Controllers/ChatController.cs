﻿using ChatApp.Domain.Entities;
using ChatApp.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] ChatSession chatSession)
        {
            var result = await _chatService.AddChatSessionAsync(chatSession);
            return Ok(result);
        }

        [HttpPost("poll")]
        public async Task<IActionResult> PollChat([FromBody] Guid sessionGuid)
        {
            var chatSession = await _chatService.GetChatSessionByGuidAsync(sessionGuid);

            if (chatSession == null)
            {
                return NotFound("Chat session not found.");
            }

            chatSession.LastActiveTime = DateTime.UtcNow;
            await _chatService.UpdateChatSessionAsync(chatSession);

            return Ok(new { message = "Chat session is active." });
        }

        [HttpPost("getchatsessionid")]
        public async Task<IActionResult> GetChatSessionByGuidAsync([FromBody] Guid sessionGuid)
        {
            var chatSession = await _chatService.GetChatSessionByGuidAsync(sessionGuid);

            if (chatSession == null)
            {
                return NotFound("Chat session id not found.");
            }

            return Ok(chatSession);
        }

        [HttpPut("updatechatstatus")]
        public async Task<IActionResult> UpdateChatStatus([FromBody] ChatSession chatSession)
        {
            var result = await _chatService.UpdateChatSessionStatusAsync(chatSession);

            if (result)
                return Ok(new { Message = "Status updated successfully." });

            return NotFound(new { Message = "Chat Session not found." });
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingChats()
        {
            var chats = await _chatService.GetPendingChatsAsync();
            return Ok(chats);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllChats()
        {
            var chats = await _chatService.GetAllChatsAsync();
            return Ok(chats);
        }
    }

}
