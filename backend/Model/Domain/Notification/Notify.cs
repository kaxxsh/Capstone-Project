using backend.Model.Domain.Post;
using backend.Model.Domain.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Model.Domain.Notification
{
    public class Notify
    {
        [Key]
        public Guid NotifyId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsRead { get; set; }

        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }
        public UserDetails FromUser { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public UserDetails User { get; set; }
        public Guid? PostId { get; set; }
        public Notify()
        {
            DateCreated = DateTime.UtcNow;
            IsRead = false;
        }
    }
}
