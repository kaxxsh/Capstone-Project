using backend.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFollowController : ControllerBase
    {
        private readonly IUserFollowService service;

        public UserFollowController(IUserFollowService service)
        {
            this.service = service;
        }

        [HttpPost("follow/{followerName}")]
        public async Task<IActionResult> FollowUser(string followerName)
        {
            var validate = await service.IsFollowingAsync(followerName);
            if (validate == true)
            {
                return BadRequest("User is already in Following List");
            }
            var result = await service.FollowUserAsync(followerName);
            if (result == true)
            {
                return Ok("User Followed SuccessFully");
            }
            return BadRequest("User Cannot self Follow");
        }

        [HttpPost("unfollow/{followerName}")]
        public async Task<IActionResult> UnfollowUser(string followerName)
        {
            var result = await service.UnfollowUserAsync(followerName);
            if (result == true)
            {
                return Ok("User Unfollowed SuccessFully");
            }
            return BadRequest("User Cannot unFollow themself");
        }

        [HttpGet("followers/{followerName}")]
        public async Task<IActionResult> GetFollowers(string followerName)
        {
            var result = await service.GetFollowersAsync(followerName);
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            return NotFound("No followers found.");
        }

        [HttpGet("following/{followingName}")]
        public async Task<IActionResult> GetFollowing(string followingName)
        {
            var result = await service.GetFollowingAsync(followingName);
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            return NotFound("No following found.");
        }


        [HttpGet("isfollowing/{followerName}")]
        public async Task<IActionResult> IsFollowing(string followerName)
        {
            var result = await service.IsFollowingAsync(followerName);
            if (result == true)
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }
}
