using backend.Interface.Services;
using backend.Model.Dtos.PostFeed;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("GetPostByUser/{UserID}")]
        public async Task<IActionResult> GetPostByUserAsync(string UserID)
        {
            var post = await _services.GetUserPostsAndRetweets(UserID);
            if (post == null)
            {
                return BadRequest();
            }
            return Ok(post);
        }

        [HttpGet("hashtag/{tag}")]
        public async Task<IActionResult> GetPostsByHashtag(string tag)
        {
            var posts = await _services.GetPostsByHashtagAsync(tag);
            return Ok(posts);
        }

        [HttpGet("hashtags")]
        public async Task<IActionResult> GetAllHashtags()
        {
            var hashtags = await _services.GetAllHashtagsAsync();
            return Ok(hashtags);
        }

        [HttpGet("GetPostByUserFollowed")]
        public async Task<IActionResult> GetPostByUserFollowed()
        {
            var posts = await _services.GetPostsByUserFollowed();
            return Ok(posts);
        }

        [HttpGet("GetAllPostsAndRetweets")]
        public async Task<IActionResult> GetAllPostsAndRetweets()
        {
            var posts = await _services.GetAllPostsAndRetweets();
            return Ok(posts);
        }
    }
}
