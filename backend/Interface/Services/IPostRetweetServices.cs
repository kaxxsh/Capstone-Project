using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed.RetweetPost;
using System;
using System.Threading.Tasks;

namespace backend.Interface.Services
{
    public interface IPostRetweetServices
    {
        Task<PostRetweetResponseDto> AddPostRetweetAsync(PostRetweetRequestDto postRetweetDto);
        Task<PostRetweetResponseDto> GetPostRetweetByIdAsync(Guid postRetweetId);
        Task<IEnumerable<PostRetweetResponseDto>> GetPostRetweetsByPostIdAsync(Guid postId);
        Task<IEnumerable<PostRetweetResponseDto>> GetPostRetweetsByUserIdAsync(string userId);
        Task<bool> DeletePostRetweetAsync(Guid postRetweetId);
    }
}
