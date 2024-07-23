using backend.Model.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Domain.Follow
{
    public class UserFollow
    {
        public Guid Id { get; set; }

        [ForeignKey("FollowerUser")]
        public string FollowerUserId { get; set; }
        public UserDetails FollowerUser { get; set; }

        [ForeignKey("FollowedUser")]
        public string FollowedUserId { get; set; }
        public UserDetails FollowedUser { get; set; }
        public DateTime FollowDate { get; set; }
    }
}
