using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed;
using backend.Model.Dtos.PostFeed.RetweetPost;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Services
{
    public class PostRetweetService : IPostRetweetServices
    {
        private readonly IPostRetweetRepository repository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PostRetweetService(IPostRetweetRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<PostRetweetResponseDto> AddPostRetweetAsync(PostRetweetRequestDto postRetweetDto)
        {
            try
            {
                var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);
                var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                var data = new PostRetweet
                {
                    PostId = postRetweetDto.PostId,
                    UserId = userId,
                    RetweetContent = postRetweetDto.RetweetContent,
                    RetweetDate = DateTime.Now
                };
                var result = await repository.Create(mapper.Map<PostRetweet>(data));
                return mapper.Map<PostRetweetResponseDto>(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeletePostRetweetAsync(Guid postRetweetId)
        {
            try
            {
                var result = await repository.Delete(postRetweetId);
                return result != null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PostRetweetResponseDto> GetPostRetweetByIdAsync(Guid postRetweetId)
        {
            try
            {
                var result = await repository.GetById(postRetweetId);
                return mapper.Map<PostRetweetResponseDto>(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<PostRetweetResponseDto>> GetPostRetweetsByPostIdAsync(Guid postId)
        {
            try
            {
                var result = await repository.GetPostRetweetsByPost(postId);
                return mapper.Map<IEnumerable<PostRetweetResponseDto>>(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<PostRetweetResponseDto>> GetPostRetweetsByUserIdAsync(string userId)
        {
            try
            {
                var result = await repository.GetPostRetweetsByUserId(userId);
                return mapper.Map<IEnumerable<PostRetweetResponseDto>>(result);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
