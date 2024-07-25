using backend.Model.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Model.Domain.Post
{
    public class PostRetweet
    {
        [Key]
        public Guid RetweetId { get; set; }

        [ForeignKey("PostFeed")]
        public Guid PostId { get; set; }
        public PostFeed PostFeed { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public UserDetails User { get; set; }

        public string? RetweetContent { get; set; }
        public DateTime RetweetDate { get; set; }
    }
}
