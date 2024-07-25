using backend.Context;
using backend.Interface.Repository;
using backend.Model.Domain.Post;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class FeedPostRepository : IPostFeedRepository
    {
        private readonly ApplicationDbContext _context;

        public FeedPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PostFeed> Create(PostFeed entity)
        {
            _context.Posts.Add(entity);
            await _context.SaveChangesAsync();
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
            return post;
        }

        public async Task<IEnumerable<PostFeed>> GetAll()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostComments)
                .Include(p => p.PostLikes)
                .Include(p => p.PostRetweets)
                .ToListAsync();
        }

        public async Task<PostFeed> GetById(Guid id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostComments)
                .Include(p => p.PostLikes)
                .Include(p => p.PostRetweets)
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
                .ToListAsync();
        }

    }
}
