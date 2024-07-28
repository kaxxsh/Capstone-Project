namespace backend.Model.Dtos.PostFeed
{
    public class HashTagDto
    {
        public Guid HashtagId { get; set; }
        public string Tag { get; set; }
        public int Count { get; set; }
    }
}
