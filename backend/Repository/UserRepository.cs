using backend.Context;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.User;
using backend.Model.Dtos.Notify;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotifyServices _notify;

        public UserRepository(ApplicationDbContext context, INotifyServices notify)
        {
            _context = context;
            _notify = notify;
        }
        public async Task<UserDetails> Create(UserDetails entity)
        {
            _context.UserDetails.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<UserDetails> Delete(string id)
        {
            var user = await _context.UserDetails.FindAsync(id);
            if (user != null)
            {
                _context.UserDetails.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public async Task<IEnumerable<UserDetails>> GetAll()
        {
            return await _context.UserDetails.Include(x => x.Followers).Include(x => x.Following).ToListAsync();
        }

        public async Task<UserDetails> GetById(string id)
        {
            return await _context.UserDetails.Include(x => x.Followers).ThenInclude(xs => xs.FollowerUser).Include(x => x.Following).ThenInclude(xs => xs.FollowedUser).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserDetails> GetUserByNameAsync(string Name)
        {
            return await _context.UserDetails.Include(x => x.Followers).ThenInclude(xs => xs.FollowerUser).Include(x => x.Following).ThenInclude(xs => xs.FollowedUser).FirstOrDefaultAsync(x => x.Name == Name);
        }

        public async Task<UserDetails> GetUserByUserNameAsync(string UserName)
        {
            return await _context.UserDetails.Include(x => x.Followers).ThenInclude(xs => xs.FollowerUser).Include(x => x.Following).ThenInclude(xs=> xs.FollowedUser).FirstOrDefaultAsync(x => x.UserName == UserName);
        }

        public async Task<UserDetails> Update(string id, UserDetails entity)
        {
            var user = await _context.UserDetails.FindAsync(id);
            if (user != null)
            {
                user.Name = entity.Name;
                user.UserName = entity.UserName;
                user.Email = entity.Email;
                user.ProfileImage = entity.ProfileImage;
                await _context.SaveChangesAsync();

                var notification = new NotifyRequestDto
                {
                    UserId = user.Id,
                    Content = "Your profile has been updated",
                };

                return user;
            }
            return null;
        }
    }
}
