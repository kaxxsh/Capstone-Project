using System.Threading.Tasks;
using backend.Context;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Follow;
using backend.Model.Dtos.Notify;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class UserFollowRepository : IUserFollowRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotifyServices _notify;

        public UserFollowRepository(ApplicationDbContext context, INotifyServices notify)
        {
            _context = context;
            _notify = notify;
        }

        public async Task<UserFollow> FollowUser(string followerName, string followedId)
        {
            var follower = await _context.Users.SingleOrDefaultAsync(u => u.UserName == followerName);
            if (follower == null || follower.Id == followedId)
            {
                return null;
            }

            var userFollow = new UserFollow
            {
                FollowerUserId = follower.Id,
                FollowedUserId = followedId,
                FollowDate = DateTime.Now
            };

            await _context.UserFollows.AddAsync(userFollow);
            await _context.SaveChangesAsync();

            follower.FollowingCount++;
            await _context.SaveChangesAsync();

            var followedUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == followedId);
            if (followedUser != null)
            {
                followedUser.FollowersCount++;
                await _context.SaveChangesAsync();
            }

            var notification = new NotifyRequestDto
            {
                UserId = followedId,
                Content = $"{follower.UserName} followed you",
            };

            await _notify.CreateNotificationAsync(notification);

            return userFollow;
        }

        public async Task<IEnumerable<UserFollow>> GetFollowers(string followerName, string followedId)
        {
            var following = await _context.Users.SingleOrDefaultAsync(u => u.UserName == followerName);
            if (following == null)
            {
                return null;
            }

            var userFollows = await _context.UserFollows
                .Include(u => u.FollowedUser)
                .Where(u => u.FollowerUserId == following.Id)
                .ToListAsync();

            return userFollows;
        }

        public async Task<IEnumerable<UserFollow>> GetFollowing(string followingName, string followedId)
        {
            var follower = await _context.Users.SingleOrDefaultAsync(u => u.UserName == followingName);
            if (follower == null)
            {
                return null;
            }

            var userFollows = await _context.UserFollows
                .Include(u => u.FollowerUser)
                .Where(u => u.FollowedUserId == follower.Id)
                .ToListAsync();

            return userFollows;
        }

        public async Task<bool> IsFollowing(string followerName, string followedId)
        {
            var follower = await _context.Users.SingleOrDefaultAsync(u => u.UserName == followerName);
            if (follower == null)
            {
                return false;
            }

            var userFollow = await _context.UserFollows
                .SingleOrDefaultAsync(u => u.FollowerUserId == follower.Id && u.FollowedUserId == followedId);

            return userFollow != null;
        }

        public async Task<UserFollow> UnfollowUser(string followerName, string followedId)
        {
            var follower = await _context.Users.SingleOrDefaultAsync(u => u.UserName == followerName);
            if (follower == null || follower.Id == followedId)
            {
                return null;
            }

            var userFollow = await _context.UserFollows
                .SingleOrDefaultAsync(u => u.FollowerUserId == follower.Id && u.FollowedUserId == followedId);

            if (userFollow != null)
            {
                _context.UserFollows.Remove(userFollow);
                await _context.SaveChangesAsync();

                follower.FollowingCount--;
                await _context.SaveChangesAsync();

                var followedUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == followedId);
                if (followedUser != null)
                {
                    followedUser.FollowersCount--;
                    await _context.SaveChangesAsync();
                }
            }

            var notification = new NotifyRequestDto
            {
                UserId = followedId,
                Content = $"{follower.UserName} unfollowed you",
            };

            await _notify.CreateNotificationAsync(notification);

            return userFollow;
        }
    }
}
