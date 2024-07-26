using backend.Interface.Services;
using backend.Model.Dtos.Notify;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotifyServices _notifyService;

        public NotificationController(INotifyServices notifyService)
        {
            _notifyService = notifyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotifyRequestDto notifyRequestDto)
        {
            var result = await _notifyService.CreateNotificationAsync(notifyRequestDto);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(Guid id)
        {
            var notification = await _notifyService.GetNotificationByIdAsync(id);
            if (notification != null)
            {
                return Ok(notification);
            }
            return NotFound();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            var notifications = await _notifyService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(Guid id, [FromBody] NotifyRequestDto notifyRequestDto)
        {
            var result = await _notifyService.UpdateNotificationAsync(id, notifyRequestDto);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var result = await _notifyService.DeleteNotificationAsync(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
