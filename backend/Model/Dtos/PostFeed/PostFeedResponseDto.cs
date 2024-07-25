using backend.Model.Domain.Post;
using backend.Model.Domain.User;
using backend.Model.Dtos.PostFeed.CommentPost;
using backend.Model.Dtos.PostFeed.LikePost;
using backend.Model.Dtos.PostFeed.RetweetPost;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Dtos.PostFeed
{
    public class PostFeedResponseDto
    {
        public Guid PostId { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int RetweetsCount { get; set; }
        public ICollection<PostCommentResponseDto> PostComments { get; set; }
        public ICollection<LikePostResponseDto> PostLikes { get; set; }
        public ICollection<PostRetweetDto> PostRetweets { get; set; }
    }
}
