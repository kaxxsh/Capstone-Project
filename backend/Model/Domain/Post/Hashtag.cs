using System.ComponentModel.DataAnnotations;

namespace backend.Model.Domain.Post
{
    public class Hashtag
    {
        [Key]
        public Guid HashtagId { get; set; }
        public string Tag { get; set; }
        public int Count { get; set; }
        public ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
    }
}
