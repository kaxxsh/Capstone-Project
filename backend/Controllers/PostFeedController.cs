using backend.Interface.Services;
using backend.Model.Dtos.PostFeed;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostFeedController : ControllerBase
    {
        private readonly IPostFeedServices _services;

        public PostFeedController(IPostFeedServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostsAsync()
        {
            var posts = await _services.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostAsync(Guid postId)
        {
            var post = await _services.GetPostAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostAsync([FromBody] PostFeedRequestDto postFeedRequestDto)
        {
            var post = await _services.CreatePostAsync(postFeedRequestDto);
            if (post == null)
            {
                return BadRequest();
            }
            return Ok(post);
        }


        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePostAsync(Guid postId, [FromBody] PostFeedRequestDto postFeedRequestDto)
        {
            var post = await _services.UpdatePostAsync(postId, postFeedRequestDto);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePostAsync(Guid postId)
        {
            var result = await _services.DeletePostAsync(postId);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("GetPostByUser{UserID}")]
        public async Task<IActionResult> GetPostByUserAsync(string UserID)
        {
            var post = await _services.GetUserPostsAndRetweets(UserID);
            if (post == null)
            {
                return BadRequest();
            }
            return Ok(post);
        }
    }
}
