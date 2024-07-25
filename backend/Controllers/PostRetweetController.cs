using backend.Interface.Services;
using backend.Model.Dtos.PostFeed.RetweetPost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostRetweetController : ControllerBase
    {
        private readonly IPostRetweetServices _services;

        public PostRetweetController(IPostRetweetServices services)
        {
            _services = services;
        }

        [HttpPost]
        public async Task<IActionResult> AddPostRetweet([FromBody] PostRetweetRequestDto postRetweetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _services.AddPostRetweetAsync(postRetweetDto);
                return Ok("Post Retweet Success");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("{postRetweetId}")]
        public async Task<IActionResult> DeletePostRetweet(Guid postRetweetId)
        {
            try
            {
                var result = await _services.DeletePostRetweetAsync(postRetweetId);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{postRetweetId}")]
        public async Task<IActionResult> GetPostRetweetById(Guid postRetweetId)
        {
            try
            {
                var result = await _services.GetPostRetweetByIdAsync(postRetweetId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetPostRetweetsByPostId(Guid postId)
        {
            try
            {
                var result = await _services.GetPostRetweetsByPostIdAsync(postId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostRetweetsByUserId(string userId)
        {
            try
            {
                var result = await _services.GetPostRetweetsByUserIdAsync(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
