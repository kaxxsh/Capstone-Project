using backend.Interface.Services;
using backend.Model.Dtos.PostFeed.LikePost;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostLikeController : ControllerBase
    {
        private readonly IPostLikeService service;

        public PostLikeController(IPostLikeService service)
        {
            this.service = service;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> LikeOnPost(Guid postId)
        {
            try
            {
                var result = await service.GetAllLikesOnPostAsync(postId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving likes.");
            }
        }

        [HttpPost("{postId}")]
        public async Task<IActionResult> LikePost(Guid postId)
        {
            try
            {
                var result = await service.LikePostAsync(postId);
                if (result == null)
                {
                    return Ok("Post unliked");
                }
                return Ok("Post liked");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Incorrect Post Id");
            }
        }
    }
}
