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
        private readonly ApplicationDbContext context;
        private readonly INotifyServices notify;

        public PostRetweetRepository(ApplicationDbContext context, INotifyServices notify)
        {
            this.context = context;
            this.notify = notify;
        }
        public async Task<PostRetweet> Create(PostRetweet entity)
        {
            try
            {
                var post = await context.Posts.FirstOrDefaultAsync(x => x.PostId == entity.PostId);
                if (entity.UserId == post.UserId)
                {
                    throw new Exception("You can't retweet your own post.");
                }
                var result = await context.Retweets.AddAsync(entity);
                await context.SaveChangesAsync();
                post.RetweetsCount++;
                await context.SaveChangesAsync();

                var notification = new NotifyRequestDto
                {
                    Content = "Retweeted your post",
                    UserId = post.UserId
                };

                await notify.CreateNotificationAsync(notification);
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
                var postRetweet = await context.Retweets.FindAsync(id);
                if (postRetweet == null)
                {
                    return null;
                }
                context.Retweets.Remove(postRetweet);
                await context.SaveChangesAsync();
                var post = await context.Posts.FirstOrDefaultAsync(x => x.PostId == postRetweet.PostId);
                post.RetweetsCount--;
                await context.SaveChangesAsync();

                var notification = new NotifyRequestDto
                {
                    Content = "Unretweeted your post",
                    UserId = post.UserId
                };

                await notify.CreateNotificationAsync(notification);
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
                return await context.Retweets.Include(x => x.User).OrderByDescending(x => x.RetweetDate).ToListAsync();
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
                return await context.Retweets
                    .Include(x => x.PostFeed)
                    .Include(x => x.PostFeed.PostLikes)
                    .Include(x => x.PostFeed.PostComments)
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
                var post = await context.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
                if (post == null)
                {
                    return null;
                }
                var result = await context.Retweets
                    .Include(x => x.PostFeed)
                    .Include(x => x.PostFeed.PostLikes)
                    .Include(x => x.PostFeed.PostComments)
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
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    return null;
                }
                var result = await context.Retweets
                    .Include(x => x.PostFeed)
                    .Include(x => x.PostFeed.PostLikes)
                    .Include(x => x.PostFeed.PostComments)
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
                var postRetweet = await context.Retweets.FindAsync(id);
                if (postRetweet == null)
                {
                    return null;
                }
                postRetweet.RetweetContent = entity.RetweetContent;
                postRetweet.RetweetDate = entity.RetweetDate;
                await context.SaveChangesAsync();

                var notification = new NotifyRequestDto
                {
                    Content = "Updated your retweet",
                    UserId = postRetweet.UserId
                };

                await notify.CreateNotificationAsync(notification);
                return postRetweet;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
