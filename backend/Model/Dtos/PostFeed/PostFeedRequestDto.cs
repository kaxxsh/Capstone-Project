using backend.Model.Domain.Post;
using backend.Model.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Dtos.PostFeed
{
    public class PostFeedRequestDto
    {
        public string? Content { get; set; }
        public string? Image { get; set; }
        public List<string>? Hashtags { get; set; } = new List<string>();
    }
}
