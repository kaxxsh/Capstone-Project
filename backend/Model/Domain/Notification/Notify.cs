using backend.Model.Domain.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Domain.Notification
{
    public class Notify
    {
        public Guid NotifyId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public UserDetails User { get; set; }
    }
}
