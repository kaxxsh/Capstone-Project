using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed.LikePost;

namespace backend.Interface.Services
{
    public interface IPostLikeService
    {
        Task<PostLike> LikePostAsync(Guid postId);
        Task<IEnumerable<LikePostResponseDto>> GetAllLikesOnPostAsync(Guid postId);
    }
}
