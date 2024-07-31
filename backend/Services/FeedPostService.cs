using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed;
using backend.Model.Dtos.PostFeed.CommentPost;
using backend.Model.Dtos.PostFeed.LikePost;
using backend.Model.Dtos.PostFeed.RetweetPost;
using Microsoft.Extensions.Hosting;
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

            try
            {
                var post = _mapper.Map<PostFeed>(postFeedRequestDto);
                post.UserId = userId;
                post.DateCreated = DateTime.Now;
                post.DateUpdated = DateTime.Now;

                foreach (var tag in postFeedRequestDto.Hashtags)
                {
                    var hashtag = await _repository.GetOrCreateHashtagAsync(tag);
                    post.PostHashtags.Add(new PostHashtag
                    {
                        HashtagId = hashtag.HashtagId,
                        Post = post
                    });
                }

                var result = await _repository.Create(post);
                return _mapper.Map<PostFeedResponseDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> DeletePostAsync(Guid postId)
        {
            try
            {
                var post = await _repository.GetById(postId);
                if (post == null)
                {
                    return false;
                }

                var result = await _repository.Delete(postId);
                return result != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<PostFeedResponseDto> GetPostAsync(Guid postId)
        {
            try
            {
                var post = await _repository.GetById(postId);
                return _mapper.Map<PostFeedResponseDto>(post);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<PostFeedResponseDto>> GetAllPostsAsync()
        {
            try
            {
                var posts = await _repository.GetAll();
                var sortedPosts = posts.OrderByDescending(post => post.DateUpdated);
                return _mapper.Map<IEnumerable<PostFeedResponseDto>>(sortedPosts);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<PostFeedResponseDto> UpdatePostAsync(Guid postId, PostFeedRequestDto postFeedRequestDto)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<CombinedPostViewModel>> GetUserPostsAndRetweets(string userId)
        {
            try
            {
                var originalPosts = await _repository.GetPostsByUserAsync(userId);
                var originalPostViewModels = originalPosts.Select(post => new CombinedPostViewModel
                {
                    Post = _mapper.Map<PostFeedResponseDto>(post),
                    IsRetweet = false,
                    RetweetContent = null,
                    RetweetedBy = null
                }).ToList();

                var retweetedPosts = await _repository.GetRetweetsByUserAsync(userId);
                var retweetPostViewModels = retweetedPosts.Select(retweet => new CombinedPostViewModel
                {
                    Post = _mapper.Map<PostFeedResponseDto>(retweet.PostFeed),
                    IsRetweet = true,
                    RetweetContent = retweet.RetweetContent,
                    RetweetedBy = retweet.User.UserName
                }).ToList();

                var allPosts = originalPostViewModels.Concat(retweetPostViewModels)
                                                     .OrderByDescending(p => p.Post.DateCreated)
                                                     .ToList();

                return allPosts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<List<CombinedPostViewModel>> GetAllPostsAndRetweets()
        {
            try
            {
                var originalPosts = await _repository.GetAll();
                var originalPostViewModels = originalPosts.Select(post => new CombinedPostViewModel
                {
                    Post = _mapper.Map<PostFeedResponseDto>(post),
                    IsRetweet = false,
                    RetweetContent = null,
                    RetweetedBy = null
                }).ToList();

                var retweetedPosts = await _repository.GetAllRetweet();
                var retweetPostViewModels = retweetedPosts.Select(retweet => new CombinedPostViewModel
                {
                    Post = _mapper.Map<PostFeedResponseDto>(retweet.PostFeed),
                    IsRetweet = true,
                    RetweetContent = retweet.RetweetContent,
                    RetweetedBy = retweet.User.UserName
                }).ToList();

                var allPosts = originalPostViewModels.Concat(retweetPostViewModels)
                                                     .OrderByDescending(p => p.Post.DateCreated)
                                                     .ToList();

                return allPosts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<IEnumerable<PostFeedResponseDto>> GetPostsByHashtagAsync(string hashtag)
        {
            var posts = await _repository.GetPostsByHashtagAsync(hashtag);
            return _mapper.Map<IEnumerable<PostFeedResponseDto>>(posts);
        }

        public async Task<IEnumerable<HashTagDto>> GetAllHashtagsAsync()
        {
            var hashtags = await _repository.GetAllHashtagsAsync();
            return _mapper.Map<IEnumerable<HashTagDto>>(hashtags);
        }

        public async Task<IEnumerable<PostFeedResponseDto>> GetPostsByUserFollowed()
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

            var posts = await _repository.GetPostsByUserFollowed(userId);
            return _mapper.Map<IEnumerable<PostFeedResponseDto>>(posts);
        }
    }
}
