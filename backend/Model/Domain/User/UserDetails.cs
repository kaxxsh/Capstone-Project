
using backend.Model.Domain.Follow;
using backend.Model.Domain.Notification;
using backend.Model.Domain.Post;

namespace backend.Model.Domain.User
{
    public class UserDetails
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }

        public List<PostFeed> Posts { get; set; }
        public ICollection<PostLike> PostLikes { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
        public ICollection<UserFollow> Followers { get; set; }
        public ICollection<UserFollow> Following { get; set; }
        public ICollection<Notify> Notifies { get; set; }
    }
}
