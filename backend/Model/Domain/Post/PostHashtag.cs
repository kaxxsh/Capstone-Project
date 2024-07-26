namespace backend.Model.Domain.Post
{
    public class PostHashtag
    {
        public Guid PostId { get; set; }
        public PostFeed Post { get; set; }

        public Guid HashtagId { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}
