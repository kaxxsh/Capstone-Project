using backend.Model.Domain.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Interface.Repository
{
    public interface INotifyRepository
    {
        Task<bool> CreateNotificationAsync(Notify notification);
        Task<Notify> GetNotificationByIdAsync(Guid id);
        Task<IEnumerable<Notify>> GetUserNotificationsAsync(string userId);
        Task<bool> UpdateNotificationAsync(Notify notification);
        Task<bool> DeleteNotificationAsync(Guid id);
    }
}
