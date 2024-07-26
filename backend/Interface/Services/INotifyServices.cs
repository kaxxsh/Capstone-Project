using backend.Model.Dtos.Notify;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interface.Services
{
    public interface INotifyServices
    {
        Task<bool> CreateNotificationAsync(NotifyRequestDto notifyRequestDto);
        Task<NotifyResponseDto> GetNotificationByIdAsync(Guid id);
        Task<IEnumerable<NotifyResponseDto>> GetUserNotificationsAsync(string userId);
        Task<bool> UpdateNotificationAsync(Guid id, NotifyRequestDto notifyRequestDto);
        Task<bool> DeleteNotificationAsync(Guid id);
    }
}
