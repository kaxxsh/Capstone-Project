using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed.RetweetPost;

namespace backend.Interface.Repository
{
    public interface IPostRetweetRepository: IRepository<Guid,PostRetweet>
    {
        Task<IEnumerable<PostRetweet>> GetPostRetweetsByPost(Guid postId);
        Task<IEnumerable<PostRetweet>> GetPostRetweetsByUserId(string userId);
    }
}
