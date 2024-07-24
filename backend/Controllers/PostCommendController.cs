using backend.Interface.Services;
using backend.Model.Dtos.PostFeed.CommentPost;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommendController : ControllerBase
    {
        private readonly IPostCommendServices services;

        public PostCommendController(IPostCommendServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> AddPostCommendAsync([FromBody] PostCommentRequestDto postComment)
        {
            try
            {
                var result = await services.AddPostCommendAsync(postComment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{PostCommentId}")]
        public async Task<IActionResult> UpdatePostCommendAsync([FromBody] PostCommentRequestDto postComment, Guid PostCommentId)
        {
            try
            {
                var result = await services.UpdatePostCommendAsync(PostCommentId, postComment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{PostCommentId}")]
        public async Task<IActionResult> DeletePostCommendAsync(Guid PostCommentId)
        {
            try
            {
                var result = await services.DeletePostCommendAsync(PostCommentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Use a more specific route segment or query parameter to differentiate these methods
        [HttpGet("comment/{PostCommentId}")]
        public async Task<IActionResult> GetPostCommendAsync(Guid PostCommentId)
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
        public async Task<IActionResult> GetPostCommendsAsync(Guid PostId)
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
