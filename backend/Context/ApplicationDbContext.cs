using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using backend.Model.Domain.Chat;
using backend.Model.Domain.Follow;
using backend.Model.Domain.Notification;
using backend.Model.Domain.Post;
using backend.Model.Domain.User;

namespace backend.Context
{
    public class ApplicationDbContext : IdentityDbContext<UserDetails>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for various entities
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<PostFeed> Posts { get; set; }
        public DbSet<PostLike> Likes { get; set; }
        public DbSet<PostComment> Comments { get; set; }
        public DbSet<PostRetweet> Retweets { get; set; }
        public DbSet<Notify> Notifies { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<UserConversation> UserConversations { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<PostHashtag> PostHashtags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure PostFeed entity
            builder.Entity<PostFeed>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            // Configure PostLike entity
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

            // Configure PostComment entity
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

            // Configure PostRetweet entity
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

            // Configure Notify entity
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

            // Configure UserFollow entity
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

            // Configure Message entity
            builder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Conversation entity
            builder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Conversation>()
                .HasMany(c => c.UserConversations)
                .WithOne(uc => uc.Conversation)
                .HasForeignKey(uc => uc.ConversationId);

            // Configure UserConversation entity
            builder.Entity<UserConversation>()
                .HasKey(uc => new { uc.UserId, uc.ConversationId });

            builder.Entity<UserConversation>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserConversations)
                .HasForeignKey(uc => uc.UserId);

            builder.Entity<UserConversation>()
                .HasOne(uc => uc.Conversation)
                .WithMany(c => c.UserConversations)
                .HasForeignKey(uc => uc.ConversationId);

            // Configure Hashtag entity
            builder.Entity<Hashtag>()
                .HasIndex(h => h.Tag)
                .IsUnique();

            // Configure PostHashtag entity
            builder.Entity<PostHashtag>()
                .HasKey(ph => new { ph.PostId, ph.HashtagId });

            builder.Entity<PostHashtag>()
                .HasOne(ph => ph.Post)
                .WithMany(p => p.PostHashtags)
                .HasForeignKey(ph => ph.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostHashtag>()
                .HasOne(ph => ph.Hashtag)
                .WithMany(h => h.PostHashtags)
                .HasForeignKey(ph => ph.HashtagId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure UserDetails and UserFollow relationship
            builder.Entity<UserDetails>()
                .HasMany(u => u.Followers)
                .WithOne(f => f.FollowedUser)
                .HasForeignKey(f => f.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserDetails>()
                .HasMany(u => u.Following)
                .WithOne(f => f.FollowerUser)
                .HasForeignKey(f => f.FollowerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
