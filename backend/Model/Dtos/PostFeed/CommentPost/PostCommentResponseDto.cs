namespace backend.Model.Dtos.PostFeed.CommentPost
{
    public class PostCommentResponseDto
    {
        public Guid PostCommentId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
    }
}
