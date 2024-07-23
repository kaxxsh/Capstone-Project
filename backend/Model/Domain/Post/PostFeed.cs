using backend.Model.Domain.User;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Domain.Post
{
    public class PostFeed
    {
        public Guid PostId { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int RetweetsCount { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public UserDetails User { get; set; }

        public ICollection<PostLike> PostLikes { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
    }
}
