using backend.Model.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Dtos.PostFeed.RetweetPost
{
    public class PostRetweetRequestDto
    {
        public Guid PostId { get; set; }
        public string? RetweetContent { get; set; }
    }
}
