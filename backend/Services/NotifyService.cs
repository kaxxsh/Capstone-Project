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
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotifyService(INotifyRepository notifyRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _notifyRepository = notifyRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
                IsRead = false,
                PostId = notifyRequestDto.PostId

            };

            return await _notifyRepository.CreateNotificationAsync(notification);
        }

        public async Task<NotifyResponseDto> GetNotificationByIdAsync(Guid id)
        {
            try
            {
                var notification = await _notifyRepository.GetNotificationByIdAsync(id);
                if (notification == null)
                {
                    return null;
                }

                return _mapper.Map<NotifyResponseDto>(notification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<NotifyResponseDto>> GetUserNotificationsAsync(string userId)
        {
            try
            {
                var notifications = await _notifyRepository.GetUserNotificationsAsync(userId);

                return _mapper.Map<IEnumerable<NotifyResponseDto>>(notifications);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> UpdateNotificationAsync(Guid id, NotifyRequestDto notifyRequestDto)
        {
            try
            {
                var notification = await _notifyRepository.GetNotificationByIdAsync(id);
                if (notification == null)
                {
                    return false;
                }

                notification.Content = notifyRequestDto.Content;

                return await _notifyRepository.UpdateNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteNotificationAsync(Guid id)
        {
            try
            {
                return await _notifyRepository.DeleteNotificationAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

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