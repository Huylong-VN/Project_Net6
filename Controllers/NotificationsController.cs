using CRM_Management_Student.Backend.Application;
using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.SignalR;
using CRM_Management_Student.Backend.ViewModels.Common;
using CRM_Management_Student.Backend.ViewModels.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CRM_Management_Student.Backend.Controllers
{
    public class NotificationsController : BasesController
    {
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService, IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PagedAndSortedResultRequestDto request)
        {
            return Ok(await _notificationService.GetListAsync(request));
        }
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] NotificationCreate request)
        {
            request.UserCreated = User?.Identity?.Name;
            var notify = await _notificationService.CreateAsync(request);
            await _hubContext.Clients.All.SendAsync("notify", notify);
            return Ok(notify);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var notify = await _notificationService.DeleteAsync(id);
            await _hubContext.Clients.All.SendAsync("notify", notify);
            return Ok(notify);
        }
        [HttpPost("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendNotify(Guid id,string?[] UserName)
        {
            var notify = await _notificationService.GetNotifyById(id);
            await _hubContext.Clients.All.SendAsync("notifyAll",notify.ResultObj, User?.Identity?.Name, UserName);
            return Ok(notify);
        }
    }
}
