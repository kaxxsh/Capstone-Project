using Azure.Identity;
using backend.Context;
using backend.Interface.Repository;
using backend.Model.Domain.Post;
using backend.Model.Dtos.PostFeed.CommentPost;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Repository
{
    public class PostCommendRepository : IPostCommendRepository
    {
        private readonly ApplicationDbContext context;

        public PostCommendRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<PostComment> Create(PostComment entity)
        {
            try
            {
                await context.Comments.AddAsync(entity);
                await context.SaveChangesAsync();
                var post = await context.Posts.FindAsync(entity.PostId);
                post.CommentsCount++;
                await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the comment.", ex);
            }
        }

        public async Task<PostComment> Delete(Guid id)
        {
            try
            {
                var postComment = await context.Comments.FindAsync(id);
                if (postComment == null)
                {
                    throw new Exception("Post Comment not found.");
                }
                context.Comments.Remove(postComment);
                await context.SaveChangesAsync();
                var post = await context.Posts.FindAsync(postComment.PostId);
                post.CommentsCount--;
                await context.SaveChangesAsync();
                return postComment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the comment.", ex);
            }
        }

        public async Task<IEnumerable<PostComment>> GetAll()
        {
            try
            {
                return await context.Comments.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving comments.", ex);
            }
        }

        public async Task<PostComment> GetById(Guid id)
        {
            try
            {
                var postComment = await context.Comments.Include(x => x.User).FirstOrDefaultAsync(x => x.PostCommentId == id);
                if (postComment == null)
                {
                    throw new Exception("Post Comment not found.");
                }
                return postComment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the comment.", ex);
            }
        }

        public async Task<IEnumerable<PostComment>> GetCommentByPost(Guid PostId)
        {
            try
            {
                return await context.Comments.Where(c => c.PostId == PostId)
                                            .Include(x => x.User)
                                            .OrderByDescending(c => c.DateCreated)
                                            .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving comments for the post.", ex);
            }
        }

        public async Task<PostComment> Update(Guid id, PostComment entity)
        {
            try
            {
                var postComment = await context.Comments.FindAsync(id);
                if (postComment == null)
                {
                    throw new Exception("Post Comment not found.");
                }
                postComment.Content = entity.Content;
                postComment.DateCreated = entity.DateCreated;
                await context.SaveChangesAsync();
                return postComment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the comment.", ex);
            }
        }
    }
}
