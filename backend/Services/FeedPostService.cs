using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed;
using System.IdentityModel.Tokens.Jwt;
namespace backend.Services
{
    public class FeedPostService : IPostFeedServices
    {
        private readonly IPostFeedRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedPostService(IPostFeedRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PostFeedResponseDto> CreatePostAsync(PostFeedRequestDto postFeedRequestDto)
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User is not authenticated or UserId is not available.");
            }

            var post = _mapper.Map<PostFeed>(postFeedRequestDto);
            post.UserId = userId;
            post.DateCreated = DateTime.Now;
            post.DateUpdated = DateTime.Now;

            var result = await _repository.Create(post);
            return _mapper.Map<PostFeedResponseDto>(result);
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await _repository.GetById(postId);
            if (post == null)
            {
                return false;
            }

            var result = await _repository.Delete(postId);
            return result != null;
        }

        public async Task<PostFeedResponseDto> GetPostAsync(Guid postId)
        {
            var post = await _repository.GetById(postId);
            return _mapper.Map<PostFeedResponseDto>(post);
        }

        public async Task<IEnumerable<PostFeedResponseDto>> GetPostsAsync()
        {
            var posts = await _repository.GetAll();
            var sortedPosts = posts.OrderByDescending(post => post.DateUpdated);
            return _mapper.Map<IEnumerable<PostFeedResponseDto>>(sortedPosts);
        }


        public async Task<PostFeedResponseDto> UpdatePostAsync(Guid postId, PostFeedRequestDto postFeedRequestDto)
        {
            var post = await _repository.GetById(postId);
            if (post == null)
            {
                return null;
            }

            post.Content = postFeedRequestDto.Content;
            post.Image = postFeedRequestDto.Image;
            post.DateUpdated = DateTime.Now;

            var result = await _repository.Update(postId, post);
            return _mapper.Map<PostFeedResponseDto>(result);
        }
    }
}
