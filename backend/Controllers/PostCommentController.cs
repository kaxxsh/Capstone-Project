using backend.Interface.Services;
using backend.Model.Dtos.PostFeed.CommentPost;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommentController : ControllerBase
    {
        private readonly IPostCommendServices services;

        public PostCommentController(IPostCommendServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> AddPostCommentAsync([FromBody] PostCommentRequestDto postComment)
        {
            try
            {
                var result = await services.AddPostCommendAsync(postComment);
                if (result == null)
                {
                    return BadRequest("Post not found.");
                }
                return Ok("Comment Success");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{PostCommentId}")]
        public async Task<IActionResult> UpdatePostCommentAsync([FromBody] PostCommentRequestDto postComment, Guid PostCommentId)
        {
            try
            {
                var result = await services.UpdatePostCommendAsync(PostCommentId, postComment);
                if (result == null)
                {
                    return BadRequest("Post not found.");
                }
                return Ok("Comment Updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{PostCommentId}")]
        public async Task<IActionResult> DeletePostCommentAsync(Guid PostCommentId)
        {
            try
            {
                var result = await services.DeletePostCommendAsync(PostCommentId);
                if (result == null)
                {
                    return BadRequest("Post not found.");
                }
                return Ok("Comment Deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Use a more specific route segment or query parameter to differentiate these methods
        [HttpGet("comment/{PostCommentId}")]
        public async Task<IActionResult> GetPostCommentAsync(Guid PostCommentId)
        {
            try
            {
                var result = await services.GetCommendAsync(PostCommentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("post/{PostId}")]
        public async Task<IActionResult> GetPostCommentsAsync(Guid PostId)
        {
            try
            {
                var result = await services.GetPostCommendsAsync(PostId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
