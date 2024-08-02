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
        private readonly ApplicationDbContext _context;
        private readonly INotifyServices _services;
        private readonly IMapper _mapper;

        public PostLikeRepository(ApplicationDbContext context, INotifyServices services)
        {
            _context = context;
            _services = services;
        }

        public async Task<IEnumerable<LikePostResponseDto>> GetAllLikesOnPost(Guid postId)
        {
            try
            {
                return await _context.Likes
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
                throw new Exception("An error occurred while retrieving likes for the post.");
            }
        }

        public async Task<PostLike> LikePost(Guid postId, string userId)
        {
            try
            {
                var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
                if (like != null)
                {
                    _context.Likes.Remove(like);
                    await _context.SaveChangesAsync();

                    var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
                    if (post != null)
                    {
                        post.LikesCount--;
                        await _context.SaveChangesAsync();
                    }

                    var notification = new NotifyRequestDto
                    {
                        UserId = post.UserId,
                        Content = $"You unliked a post.",
                        PostId = postId
                    };
                    await _services.CreateNotificationAsync(notification);

                    return null;
                }

                var postLike = new PostLike { PostId = postId, UserId = userId };
                await _context.Likes.AddAsync(postLike);
                await _context.SaveChangesAsync();

                var updatedPost = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
                if (updatedPost != null)
                {
                    updatedPost.LikesCount++;
                    await _context.SaveChangesAsync();
                }
                var notificationDto = new NotifyRequestDto
                {
                    UserId = updatedPost.UserId,
                    Content = "You liked a post.",
                    PostId = postId
                };

                await _services.CreateNotificationAsync(notificationDto);
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
