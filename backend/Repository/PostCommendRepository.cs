using backend.Context;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.Notify;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class PostCommendRepository : IPostCommendRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotifyServices _notify;

        public PostCommendRepository(ApplicationDbContext context, INotifyServices notify)
        {
            _context = context;
            _notify = notify;
        }

        public async Task<PostComment> Create(PostComment entity)
        {
            try
            {
                await _context.Comments.AddAsync(entity);
                await _context.SaveChangesAsync();
                var post = await _context.Posts.FindAsync(entity.PostId);
                post.CommentsCount++;
                await _context.SaveChangesAsync();

                var User = await _context.Users.FindAsync(entity.UserId);

                var notification = new NotifyRequestDto
                {
                    UserId = post.UserId,
                    Content = $"{User.UserName} commented on your post.",
                    PostId = post.PostId
                };
                await _notify.CreateNotificationAsync(notification);
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
                var postComment = await _context.Comments.FindAsync(id);
                if (postComment == null)
                {
                    throw new Exception("Post Comment not found.");
                }
                _context.Comments.Remove(postComment);
                await _context.SaveChangesAsync();
                var post = await _context.Posts.FindAsync(postComment.PostId);
                post.CommentsCount--;
                await _context.SaveChangesAsync();

                var notification = new NotifyRequestDto
                {
                    UserId = post.UserId,
                    Content = $"deleted a comment on your post.",
                    PostId = post.PostId
                };

                await _notify.CreateNotificationAsync(notification);

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
                return await _context.Comments.ToListAsync();
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
                var postComment = await _context.Comments.Include(x => x.User).FirstOrDefaultAsync(x => x.PostCommentId == id);
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
                return await _context.Comments.Where(c => c.PostId == PostId)
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
                var postComment = await _context.Comments.FindAsync(id);
                if (postComment == null)
                {
                    throw new Exception("Post Comment not found.");
                }
                postComment.Content = entity.Content;
                postComment.DateCreated = entity.DateCreated;
                await _context.SaveChangesAsync();

                var User = await _context.Users.FindAsync(entity.UserId);

                var notification = new NotifyRequestDto
                {
                    UserId = postComment.UserId,
                    Content = $"updated a comment on your post.",
                    PostId = postComment.PostId
                };

                await _notify.CreateNotificationAsync(notification);

                return postComment;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the comment.", ex);
            }
        }
    }
}
