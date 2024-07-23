using backend.Model.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Domain.Post
{
    public class PostLike
    {
        public Guid PostLikeId { get; set; }

        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public PostFeed Post { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public UserDetails User { get; set; }
    }
}
