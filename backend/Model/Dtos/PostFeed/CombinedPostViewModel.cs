using backend.Model.Domain.User;

namespace backend.Model.Dtos.PostFeed
{
    public class CombinedPostViewModel
    {
        public PostFeedResponseDto Post { get; set; }
        public bool IsRetweet { get; set; }
        public string? RetweetContent { get; set; }
        public string RetweetedBy { get; set; }
    }
}
