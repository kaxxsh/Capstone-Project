using System;

namespace backend.Model.Dtos.Notify
{
    public class NotifyResponseDto
    {
        public Guid NotifyId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Guid PostId { get; set; }
    }
}
