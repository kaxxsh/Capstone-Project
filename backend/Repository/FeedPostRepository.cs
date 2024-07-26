using backend.Context;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.Notify;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class FeedPostRepository : IPostFeedRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotifyServices _notify;

        public FeedPostRepository(ApplicationDbContext context, INotifyServices notify)
        {
            _context = context;
            _notify = notify;
        }

        public async Task<PostFeed> Create(PostFeed entity)
        {
            _context.Posts.Add(entity);
            await _context.SaveChangesAsync();

            var notification = new NotifyRequestDto
            {
                UserId = entity.UserId,
                Content = "You have a new post",
            };
            await _notify.CreateNotificationAsync(notification);
            return entity;
        }

        public async Task<PostFeed> Delete(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return null;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            var notification = new NotifyRequestDto
            {
                UserId = post.UserId,
                Content = "Your post has been deleted",
            };
            await _notify.CreateNotificationAsync(notification);
            return post;
        }

        public async Task<IEnumerable<PostFeed>> GetAll()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostComments)
                .Include(p => p.PostLikes)
                .Include(p => p.PostRetweets)
                    .ThenInclude(pr => pr.User)
                .ToListAsync();
        }

        public async Task<PostFeed> GetById(Guid id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostComments)
                .Include(p => p.PostLikes)
                .Include(p => p.PostRetweets)
                    .ThenInclude(pr => pr.User)
                .FirstOrDefaultAsync(p => p.PostId == id);
        }

        public async Task<PostFeed> Update(Guid id, PostFeed entity)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return null;
            }

            post.Content = entity.Content;
            post.Image = entity.Image;
            post.DateUpdated = DateTime.Now;

            await _context.SaveChangesAsync();

            var notification = new NotifyRequestDto
            {
                UserId = post.UserId,
                Content = "Your post has been updated",
            };
            return post;
        }

        public async Task<IEnumerable<PostFeed>> GetPostsByUserAsync(string userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .Include(p => p.PostComments)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostRetweet>> GetRetweetsByUserAsync(string userId)
        {
            return await _context.Retweets
                .Where(r => r.UserId == userId)
                .Include(r => r.PostFeed)
                    .ThenInclude(pf => pf.User)
                .Include(r => r.PostFeed.PostComments)
                    .ThenInclude(pc => pc.User)
                .Include(r => r.PostFeed.PostLikes)
                    .ThenInclude(pl => pl.User)
                .Include(userId => userId.User)
                .ToListAsync();
        }

    }
}
