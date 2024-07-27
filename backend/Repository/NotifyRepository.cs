using backend.Context;
using backend.Interface.Repository;
using backend.Model.Domain.Notification;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class NotifyRepository : INotifyRepository
    {
        private readonly ApplicationDbContext _context;

        public NotifyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNotificationAsync(Notify notification)
        {
            await _context.Notifies.AddAsync(notification);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Notify> GetNotificationByIdAsync(Guid id)
        {
            var notification = await _context.Notifies.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                _context.Notifies.Update(notification);
                await _context.SaveChangesAsync();
            }
            return await _context.Notifies
                .Include(n => n.FromUser)
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.NotifyId == id);
        }

        public async Task<IEnumerable<Notify>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifies
                .Include(n => n.FromUser)
                .Include(n => n.User)
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();
        }

        public async Task<bool> UpdateNotificationAsync(Notify notification)
        {
            _context.Notifies.Update(notification);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteNotificationAsync(Guid id)
        {
            var notification = await _context.Notifies.FindAsync(id);
            if (notification == null)
            {
                return false;
            }

            _context.Notifies.Remove(notification);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}