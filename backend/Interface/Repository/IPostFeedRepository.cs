using backend.Model.Domain.Post;

namespace backend.Interface.Repository
{
    public interface IPostFeedRepository : IRepository<Guid,PostFeed>
    {
        Task<IEnumerable<PostFeed>> GetPostsByUserAsync(string userId);
        Task<IEnumerable<PostRetweet>> GetRetweetsByUserAsync(string userId);
        Task<Hashtag> GetOrCreateHashtagAsync(string tag);
        Task<IEnumerable<PostFeed>> GetPostsByHashtagAsync(string hashtag);
        Task<IEnumerable<Hashtag>> GetAllHashtagsAsync();
        Task<IEnumerable<PostFeed>> GetPostsByUserFollowed(string userId);
        Task<IEnumerable<PostRetweet>> GetAllRetweet();
    }
}
