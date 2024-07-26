using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Dtos.User.UserFollow;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Services
{
    public class UserFollowService : IUserFollowService
    {
        private readonly IUserFollowRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public UserFollowService(IUserFollowRepository repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
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

                var result = await repository.FollowUser(followerName, userId);
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
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not authenticated or Logined User.");
                }
                var result = await repository.GetFollowers(followerName, userId);
                return result.Select(r => new UserFollowDto
                {
                    UserId = r.FollowedUserId,
                    UserName = r.FollowedUser.UserName,
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
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not authenticated or Logined User.");
                }
                var result = await repository.GetFollowing(followingName, userId);
                return result.Select(r => new UserFollowDto
                {
                    UserId = r.FollowerUserId,
                    UserName = r.FollowerUser.UserName,
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
                var result = await repository.IsFollowing(followerName, userId);
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

                var result = await repository.UnfollowUser(followerName, userId);
                return result != null;
            }
            catch (Exception)
            {
                return false; 
            }
        }

        public string GetUserId()
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            return userId;
        }
    }
}
