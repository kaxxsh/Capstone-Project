using backend.Model.Domain.Post;

namespace backend.Interface.Repository
{
    public interface IPostFeedRepository : IRepository<Guid,PostFeed>
    {
        Task<IEnumerable<PostFeed>> GetPostsByUserAsync(string userId);
        Task<IEnumerable<PostRetweet>> GetRetweetsByUserAsync(string userId);
    }
}
