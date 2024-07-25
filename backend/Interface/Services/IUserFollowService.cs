using backend.Model.Domain.Follow;
using backend.Model.Dtos.User.UserFollow;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace backend.Interface.Services
{
    public interface IUserFollowService
    {
        Task<bool> FollowUserAsync(string followerName);
        Task<bool> UnfollowUserAsync(string followerName);
        Task<bool> IsFollowingAsync(string followerName);
        Task<IEnumerable<UserFollowDto>> GetFollowersAsync(string followerName);
        Task<IEnumerable<UserFollowDto>> GetFollowingAsync(string followingName);
    }
}
