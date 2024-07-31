using backend.Model.Dtos.PostFeed;

namespace backend.Interface.Services
{
    public interface IPostFeedServices
    {
        Task<PostFeedResponseDto> CreatePostAsync(PostFeedRequestDto postFeedRequestDto);
        Task<PostFeedResponseDto> GetPostAsync(Guid postId);
        Task<IEnumerable<PostFeedResponseDto>> GetAllPostsAsync();
        Task<PostFeedResponseDto> UpdatePostAsync(Guid postId, PostFeedRequestDto postFeedRequestDto);
        Task<bool> DeletePostAsync(Guid postId);
        Task<List<CombinedPostViewModel>> GetUserPostsAndRetweets(string userId);
        Task<List<CombinedPostViewModel>> GetAllPostsAndRetweets();
        Task<IEnumerable<PostFeedResponseDto>> GetPostsByHashtagAsync(string hashtag);
        Task<IEnumerable<HashTagDto>> GetAllHashtagsAsync();
        Task<IEnumerable<PostFeedResponseDto>> GetPostsByUserFollowed();
    }
}
