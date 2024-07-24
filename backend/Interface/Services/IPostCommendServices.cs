using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed.CommentPost;

namespace backend.Interface.Services
{
    public interface IPostCommendServices
    {
        Task<IEnumerable<PostCommentResponseDto>> GetPostCommendsAsync(Guid PostId);
        Task<PostCommentResponseDto> GetCommendAsync(Guid PostCommentId);
        Task<PostComment> AddPostCommendAsync(PostCommentRequestDto postComment);
        Task<PostComment> UpdatePostCommendAsync(Guid PostCommentId,PostCommentRequestDto postComment);
        Task<PostComment> DeletePostCommendAsync(Guid PostCommentId);
    }
}
