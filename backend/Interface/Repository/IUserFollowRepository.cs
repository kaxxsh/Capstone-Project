using backend.Model.Domain.Follow;

namespace backend.Interface.Repository
{
    public interface IUserFollowRepository
    {
        Task<UserFollow> FollowUser(string followerName, string followedId);
        Task<UserFollow> UnfollowUser(string followerName, string followedId);
        Task<bool> IsFollowing(string followerName,string followedId);
        Task<IEnumerable<UserFollow>> GetFollowers(string followerName);
        Task<IEnumerable<UserFollow>> GetFollowing(string followingName);
    }
}
