using backend.Context;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.Post;
using backend.Model.Dtos.Notify;
using backend.Model.Dtos.PostFeed.RetweetPost;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class PostRetweetRepository : IPostRetweetRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly INotifyServices _notify;

        public PostRetweetRepository(ApplicationDbContext context, INotifyServices notify)
        {
            _context = context;
            _notify = notify;
        }
        public async Task<PostRetweet> Create(PostRetweet entity)
        {
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == entity.PostId);
                if (entity.UserId == post.UserId)
                {
                    throw new Exception("You can't retweet your own post.");
                }
                if(await _context.Retweets.AnyAsync(x => x.PostId == entity.PostId && x.UserId == entity.UserId))
                {
                    throw new Exception("You already retweeted this post.");
                }
                var result = await _context.Retweets.AddAsync(entity);
                await _context.SaveChangesAsync();
                post.RetweetsCount++;
                await _context.SaveChangesAsync();

                var notification = new NotifyRequestDto
                {
                    Content = "Retweeted your post",
                    UserId = post.UserId,
                    PostId = post.PostId
                };

                await _notify.CreateNotificationAsync(notification);
                return result.Entity;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PostRetweet> Delete(Guid id)
        {
            try
            {
                var postRetweet = await _context.Retweets.FindAsync(id);
                if (postRetweet == null)
                {
                    return null;
                }
                _context.Retweets.Remove(postRetweet);
                await _context.SaveChangesAsync();
                var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == postRetweet.PostId);
                post.RetweetsCount--;
                await _context.SaveChangesAsync();

                var notification = new NotifyRequestDto
                {
                    Content = "Unretweeted your post",
                    UserId = post.UserId,
                    PostId = post.PostId
                };

                await _notify.CreateNotificationAsync(notification);
                return postRetweet;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<PostRetweet>> GetAll()
        {
            try
            {
                return await _context.Retweets.Include(x => x.User).OrderByDescending(x => x.RetweetDate).ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PostRetweet> GetById(Guid id)
        {
            try
            {
                return await _context.Retweets
                    .Include(x => x.PostFeed)
                    .Include(x => x.PostFeed.PostLikes)
                    .ThenInclude(x => x.User)
                    .Include(x => x.PostFeed.PostComments)
                    .ThenInclude(x => x.User)
                    .Include(x => x.User)
                    .OrderByDescending(x => x.RetweetDate)
                    .FirstOrDefaultAsync(x => x.RetweetId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the retweet by ID.", ex);
            }
        }


        public async Task<IEnumerable<PostRetweet>> GetPostRetweetsByPost(Guid postId)
        {
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
                if (post == null)
                {
                    return null;
                }
                var result = await _context.Retweets
                    .Include(x => x.PostFeed)
                    .Include(x => x.PostFeed.PostLikes)
                    .ThenInclude(x => x.User)
                    .Include(x => x.PostFeed.PostComments)
                    .ThenInclude(x => x.User)
                    .Include(x => x.User)
                    .Where(x => x.PostId == postId)
                    .OrderByDescending(x => x.RetweetDate)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<PostRetweet>> GetPostRetweetsByUserId(string userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    return null;
                }
                var result = await _context.Retweets
                    .Include(x => x.PostFeed)
                    .Include(x => x.PostFeed.PostLikes)
                    .ThenInclude(x => x.User)
                    .Include(x => x.PostFeed.PostComments)
                    .ThenInclude(x => x.User)
                    .Include(x => x.User)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.RetweetDate)
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PostRetweet> Update(Guid id, PostRetweet entity)
        {
            try
            {
                var postRetweet = await _context.Retweets.FindAsync(id);
                if (postRetweet == null)
                {
                    return null;
                }
                postRetweet.RetweetContent = entity.RetweetContent;
                postRetweet.RetweetDate = entity.RetweetDate;
                await _context.SaveChangesAsync();

                var notification = new NotifyRequestDto
                {
                    Content = "Updated your retweet",
                    UserId = postRetweet.UserId,
                    PostId = postRetweet.PostId
                };

                await _notify.CreateNotificationAsync(notification);
                return postRetweet;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
