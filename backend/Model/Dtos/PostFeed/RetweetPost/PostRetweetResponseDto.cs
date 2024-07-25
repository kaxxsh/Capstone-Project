
namespace backend.Model.Dtos.PostFeed.RetweetPost
{
    public class PostRetweetResponseDto
    {
        public Guid RetweetId { get; set; }
        public PostFeedResponseDto PostFeed { get; set; }
        public string UserName { get; set; }
        public string? RetweetContent { get; set; }
        public DateTime RetweetDate { get; set; }
    }
}
