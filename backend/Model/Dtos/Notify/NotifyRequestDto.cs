using System;

namespace backend.Model.Dtos.Notify
{
    public class NotifyRequestDto
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
