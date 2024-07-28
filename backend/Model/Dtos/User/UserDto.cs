using backend.Model.Domain.Notification;
using backend.Model.Dtos.PostFeed;
using backend.Model.Dtos.User.UserFollow;

namespace backend.Model.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public ICollection<UserFollowDto> Followers { get; set; }
        public ICollection<UserFollowDto> Following { get; set; }
    }
}
