using backend.Model.Domain.Follow;
using backend.Model.Domain.Notification;
using backend.Model.Domain.Post;
using backend.Model.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Context
{
    public class ApplicationDbContext : IdentityDbContext<UserDetails>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<PostFeed> Posts { get; set; }
        public DbSet<PostLike> Likes { get; set; }
        public DbSet<PostComment> Comments { get; set; }
        public DbSet<PostRetweet> Retweets { get; set; }
        public DbSet<Notify> Notifies { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostFeed>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            builder.Entity<PostLike>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(pl => pl.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostLike>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(pl => pl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PostComment>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.PostComments)
                .HasForeignKey(pc => pc.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostComment>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.PostComments)
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PostRetweet>()
                .HasOne(pr => pr.PostFeed)
                .WithMany(p => p.PostRetweets)
                .HasForeignKey(pr => pr.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostRetweet>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.PostRetweets)
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notify>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifies)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notify>()
                .HasOne(n => n.FromUser)
                .WithMany(u => u.SentNotifies)
                .HasForeignKey(n => n.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserFollow>()
                .HasOne(uf => uf.FollowerUser)
                .WithMany(u => u.Following)
                .HasForeignKey(uf => uf.FollowerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserFollow>()
                .HasOne(uf => uf.FollowedUser)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
