using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Notification;
using backend.Model.Dtos.Notify;
using System.IdentityModel.Tokens.Jwt;
namespace backend.Services
{
    public class NotifyService : INotifyServices
    {
        private readonly INotifyRepository _notifyRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NotifyService(INotifyRepository notifyRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _notifyRepository = notifyRepository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CreateNotificationAsync(NotifyRequestDto notifyRequestDto)
        {

            var notification = new Notify
            {
                NotifyId = Guid.NewGuid(),
                Content = notifyRequestDto.Content,
                FromUserId = GetUserId(),
                UserId = notifyRequestDto.UserId,
                DateCreated = DateTime.UtcNow,
                IsRead = false
            };

            return await _notifyRepository.CreateNotificationAsync(notification);
        }

        public async Task<NotifyResponseDto> GetNotificationByIdAsync(Guid id)
        {
            var notification = await _notifyRepository.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return null;
            }

            return mapper.Map<NotifyResponseDto>(notification);
        }

        public async Task<IEnumerable<NotifyResponseDto>> GetUserNotificationsAsync(string userId)
        {
            var notifications = await _notifyRepository.GetUserNotificationsAsync(userId);

            return mapper.Map<IEnumerable<NotifyResponseDto>>(notifications);
        }

        public async Task<bool> UpdateNotificationAsync(Guid id, NotifyRequestDto notifyRequestDto)
        {
            var notification = await _notifyRepository.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return false;
            }

            notification.Content = notifyRequestDto.Content;

            return await _notifyRepository.UpdateNotificationAsync(notification);
        }

        public async Task<bool> DeleteNotificationAsync(Guid id)
        {
            return await _notifyRepository.DeleteNotificationAsync(id);
        }

        public string GetUserId()
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            return userId;
        }
    }
}
