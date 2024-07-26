using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed.CommentPost;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace backend.Services
{
    public class PostCommendService : IPostCommendServices
    {
        private readonly IPostCommendRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public PostCommendService(IPostCommendRepository repository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }

        public async Task<PostComment> AddPostCommendAsync(PostCommentRequestDto postComment)
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            if (jwtToken == null) throw new Exception("JWT token not found.");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            if (userId == null) throw new Exception("User ID not found in JWT token.");

            try
            {
                var postCommentEntity = new PostComment
                {
                    PostId = postComment.PostId,
                    UserId = userId,
                    Content = postComment.Content,
                    DateCreated = DateTime.Now
                };
                return await repository.Create(postCommentEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the comment.", ex);
            }
        }

        public async Task<PostComment> DeletePostCommendAsync(Guid PostCommentId)
        {
            try
            {
                return await repository.Delete(PostCommentId);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while deleting the comment.", ex);
            }
        }

        public async Task<PostCommentResponseDto> GetCommendAsync(Guid PostCommentId)
        {
            try
            {
                var comment = await repository.GetById(PostCommentId);
                return new PostCommentResponseDto
                {
                    PostCommentId = comment.PostCommentId,
                    Content = comment.Content,
                    DateCreated = comment.DateCreated,
                    UserName = comment.User.UserName
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while retrieving the comment.", ex);
            }
        }

        public async Task<IEnumerable<PostCommentResponseDto>> GetPostCommendsAsync(Guid PostId)
        {
            try
            {
                var comments = await repository.GetCommentByPost(PostId);
                return comments.Select(x => new PostCommentResponseDto
                {
                    PostCommentId = x.PostCommentId,
                    Content = x.Content,
                    DateCreated = x.DateCreated,
                    UserName = x.User.UserName
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving comments.", ex);
            }
        }


        public async Task<PostComment> UpdatePostCommendAsync(Guid PostCommentId, PostCommentRequestDto postComment)
        {
            try
            {
                var postCommentEntity = new PostComment
                {
                    PostId = postComment.PostId,
                    Content = postComment.Content,
                    DateCreated = DateTime.Now
                };
                return await repository.Update(PostCommentId, postCommentEntity);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while updating the comment.", ex);
            }
        }
    }
}
