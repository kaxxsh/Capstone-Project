using backend.Context;
using backend.Interface.Repository;
using backend.Model.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<UserDetails> Create(UserDetails entity)
        {
            context.UserDetails.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<UserDetails> Delete(string id)
        {
            var user = await context.UserDetails.FindAsync(id);
            if (user != null)
            {
                context.UserDetails.Remove(user);
                await context.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public async Task<IEnumerable<UserDetails>> GetAll()
        {
            return await context.UserDetails.ToListAsync();
        }

        public async Task<UserDetails> GetById(string id)
        {
            return await context.UserDetails.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserDetails> GetUserByNameAsync(string Name)
        {
            return await context.UserDetails.FirstOrDefaultAsync(x => x.Name == Name);
        }

        public async Task<UserDetails> GetUserByUserNameAsync(string UserName)
        {
            return await context.UserDetails.FirstOrDefaultAsync(x => x.UserName == UserName);
        }

        public async Task<UserDetails> Update(string id, UserDetails entity)
        {
            var user = await context.UserDetails.FindAsync(id);
            if (user != null)
            {
                user.Name = entity.Name;
                user.UserName = entity.UserName;
                user.Email = entity.Email;
                user.ProfileImage = entity.ProfileImage;
                await context.SaveChangesAsync();
                return user;
            }
            return null;
        }
    }
}
