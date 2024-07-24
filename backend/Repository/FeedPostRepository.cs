using backend.Context;
using backend.Interface.Repository;
using backend.Model.Domain.Post;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _context.Posts.Include(p => p.User).ToListAsync();
        }

        public async Task<PostFeed> GetById(Guid id)
        {
            return await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.PostId == id);
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
    }
}
