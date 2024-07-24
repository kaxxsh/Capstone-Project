using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed.LikePost;

namespace backend.Interface.Repository
{
    public interface IPostLikeRepository
    {
        Task<PostLike> LikePost(Guid postId, string userId);
        Task<IEnumerable<LikePostResponseDto>> GetAllLikesOnPost(Guid postId);
    }
}
