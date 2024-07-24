using backend.Model.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Dtos.PostFeed.CommentPost
{
    public class PostCommentRequestDto
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
    }
}
