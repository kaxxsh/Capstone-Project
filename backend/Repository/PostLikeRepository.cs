using AutoMapper;
using backend.Context;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.Notify;
using backend.Model.Dtos.PostFeed.LikePost;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repository
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly ApplicationDbContext context;
        private readonly INotifyServices services;
        private readonly IMapper mapper;

        public PostLikeRepository(ApplicationDbContext context, INotifyServices services)
        {
            this.context = context;
            this.services = services;
        }

        public async Task<IEnumerable<LikePostResponseDto>> GetAllLikesOnPost(Guid postId)
        {
            try
            {
                return await context.Likes
                    .Where(p => p.PostId == postId)
                    .Select(l => new LikePostResponseDto
                    {
                        PostLikeId = l.PostLikeId,
                        UserName = l.User.UserName
                    })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                // Log the exception (optional)
                throw new Exception("An error occurred while retrieving likes for the post.");
            }
        }

        public async Task<PostLike> LikePost(Guid postId, string userId)
        {
            try
            {
                var like = await context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
                if (like != null)
                {
                    context.Likes.Remove(like);
                    await context.SaveChangesAsync();

                    var post = await context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
                    if (post != null)
                    {
                        post.LikesCount--;
                        await context.SaveChangesAsync();
                    }

                    var notification = new NotifyRequestDto
                    {
                        UserId = post.UserId,
                        Content = "You unliked a post."
                    };

                    return null;
                }

                var postLike = new PostLike { PostId = postId, UserId = userId };
                await context.Likes.AddAsync(postLike);
                await context.SaveChangesAsync();

                var updatedPost = await context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
                if (updatedPost != null)
                {
                    updatedPost.LikesCount++;
                    await context.SaveChangesAsync();
                }
                var notificationDto = new NotifyRequestDto
                {
                    UserId = updatedPost.UserId,
                    Content = "You liked a post."
                };

                return postLike;
            }
            catch (Exception e)
            {
                // Log the exception (optional)
                throw new Exception("An error occurred while liking the post.");
            }
        }
    }
}
