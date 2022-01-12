using CRM_Management_Student.Backend.Application;
using CRM_Management_Student.Backend.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CRM_Management_Student.Backend.SignalR
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SignalHub : Hub
    {
        public static List<string> Users = new List<string>();
        private readonly IUserService _userService;
        public SignalHub(IUserService userService)
        {
            _userService = userService;
        }
        public async override Task<Task> OnConnectedAsync()
        {
            Users.Add(Context?.User?.Identity?.Name??"");
            Users.ToHashSet();
            await _userService.UpdateStatus(new ViewModels.Users.UserUpdateStatus() { Status = true, Username = Context?.User?.Identity?.Name ?? "" });
            await Clients.All.SendAsync("online", Context?.User?.Identity?.Name);
            return base.OnConnectedAsync();
        }
        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            Users.Remove(Context?.User?.Identity?.Name??"");
            await _userService.UpdateStatus(new ViewModels.Users.UserUpdateStatus() { Status = false, Username = Context?.User?.Identity?.Name ?? "" });
            await Clients.All.SendAsync("offline", Context?.User?.Identity?.Name);
            return base.OnDisconnectedAsync(exception);
        }

      
    }
}