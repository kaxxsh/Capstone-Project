using backend.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("create-conversation")]
        public async Task<IActionResult> CreateConversation(string ChatWith)
        {
            var conversation = await _messageService.FindOrCreateConversationAsync(ChatWith);
            return Ok(conversation.ConversationId);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(string senderId, Guid conversationId, string content)
        {
            await _messageService.SendMessageAsync(senderId, conversationId, content);
            return Ok();
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var messages = await _messageService.GetMessagesAsync(conversationId);
            return Ok(messages);
        }

        [HttpGet("conversationsByUserId")]
        public async Task<IActionResult> GetConversations(string userId)
        {
            var conversations = await _messageService.GetConversationsForUserAsync(userId);
            return Ok(conversations);
        }

        [HttpGet("conversation")]
        public async Task<IActionResult> GetAllConversations()
        {
            var conversations = await _messageService.GetAllConversationsAsync();
            return Ok(conversations);
        }
    }


}
