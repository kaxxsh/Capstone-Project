using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Chat;
using backend.Model.Dtos.Chat;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IConversationRepository conversationRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _conversationRepository = conversationRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task SendMessageAsync(string senderId, Guid conversationId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ConversationId = conversationId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid conversationId)
        {
            var messages = await _messageRepository.GetMessagesByConversationIdAsync(conversationId);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<IEnumerable<ConversationDto>> GetConversationsForUserAsync(string userId)
        {
            var conversations = await _conversationRepository.GetConversationsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ConversationDto>>(conversations);
        }

        public async Task<ConversationDto> FindOrCreateConversationAsync(string ChatWith)
        {
            var UserId = GetUserId();
            var conversation = await _conversationRepository.GetConversationByUsersAsync(UserId, ChatWith);

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    CreatedAt = DateTime.UtcNow,
                    UserConversations = new List<UserConversation>
                {
                    new UserConversation { UserId = UserId },
                    new UserConversation { UserId = ChatWith }
                }
                };

                await _conversationRepository.AddConversationAsync(conversation);
            }

            return _mapper.Map<ConversationDto>(conversation);
        }

        public async Task<IEnumerable<ConversationUserDto>> GetAllConversationsAsync()
        {
            var conversations = await _conversationRepository.GetAllConversation();
            return _mapper.Map<IEnumerable<ConversationUserDto>>(conversations);
        }

        public string GetUserId()
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            return userId;
        }
    }
}
