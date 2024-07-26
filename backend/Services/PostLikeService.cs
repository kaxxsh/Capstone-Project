using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed.LikePost;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Services
{
    public class PostLikeService : IPostLikeService
    {
        private readonly IPostLikeRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostLikeService(IPostLikeRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<LikePostResponseDto>> GetAllLikesOnPostAsync(Guid postId)
        {
            try
            {
                return await _repository.GetAllLikesOnPost(postId);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving likes for the post.");
            }
        }

        public async Task<PostLike> LikePostAsync(Guid postId)
        {
            try
            {
                var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
                if (jwtToken == null) throw new Exception("JWT token not found.");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);
                var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                if (userId == null) throw new Exception("User ID not found in JWT token.");
                return await _repository.LikePost(postId, userId);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while liking the post.");
            }
        }
    }
}
