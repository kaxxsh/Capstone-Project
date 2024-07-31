using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Dtos.User.UserFollow;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Services
{
    public class UserFollowService : IUserFollowService
    {
        private readonly IUserFollowRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserFollowService(IUserFollowRepository repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<bool> FollowUserAsync(string followerName)
        {
            try
            {

                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not authenticated or Logined User.");
                }

                var result = await _repository.FollowUser(followerName, userId);
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<IEnumerable<UserFollowDto>> GetFollowersAsync(string followerName)
        {
            try
            {
                var result = await _repository.GetFollowers(followerName);
                return result.Select(r => new UserFollowDto
                {
                    UserId = r.FollowedUserId,
                    UserName = r.FollowedUser.UserName,
                    Name = r.FollowedUser.Name,
                    ProfileImage = r.FollowedUser.ProfileImage,
                    IsFollowed = IsFollowingAsync(r.FollowedUser.UserName).Result,
                    Bio = r.FollowedUser.Bio
                });
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserFollowDto>> GetFollowingAsync(string followingName)
        {
            try
            {
                var result = await _repository.GetFollowing(followingName);
                return result.Select(r => new UserFollowDto
                {
                    UserId = r.FollowerUserId,
                    UserName = r.FollowerUser.UserName,
                    Name = r.FollowerUser.Name,
                    ProfileImage = r.FollowerUser.ProfileImage,
                    IsFollowed = IsFollowingAsync(r.FollowerUser.UserName).Result,
                    Bio = r.FollowerUser.Bio
                });
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> IsFollowingAsync(string followerName)
        {
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not authenticated or Logined User.");
                }
                var result = await _repository.IsFollowing(followerName, userId);
                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UnfollowUserAsync(string followerName)
        {
            try
            {
                var userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not authenticated or logged in.");
                }

                var result = await _repository.UnfollowUser(followerName, userId);
                return result != null;
            }
            catch (Exception)
            {
                return false; 
            }
        }

        public string GetUserId()
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            return userId;
        }
    }
}
