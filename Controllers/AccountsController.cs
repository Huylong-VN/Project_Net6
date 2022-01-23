using CRM_Management_Student.Backend.Application;
using CRM_Management_Student.Backend.SignalR;
using CRM_Management_Student.Backend.ViewModels.Common;
using CRM_Management_Student.Backend.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CRM_Management_Student.Backend.Controllers
{
    public class AccountsController : BasesController
    {
        private readonly IUserService _userService;
        private readonly IHubContext<SignalHub> _hubContext;

        public AccountsController(IUserService userService, IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
            _userService = userService;
        }
        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginRequest request)
        {
            var result = await _userService.Authenticate(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] PagedAndSortedResultRequestDto request)
        {
            return Ok(await _userService.GetListAsync(request));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromForm] CreateUserDto request)
        {
            var result = await _userService.CreateAsync(request);
            await _hubContext.Clients.All.SendAsync("listuser", result.ResultObj);
            return Ok(result);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAsync(Guid Id,[FromForm] UpdateUserDto request)
        {
            var result = await _userService.UpdateAsync(Id, request);
            await _hubContext.Clients.All.SendAsync("listuser", result.ResultObj);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            var result = await _userService.DeleteAsync(Id);
            await _hubContext.Clients.All.SendAsync("listuser", Id);
            return result ? Ok(result) : BadRequest(result);
        }


        [HttpPut("roles/{Id}")]
        public async Task<IActionResult> RoleAssign([FromBody] RoleAssignRequest request,Guid Id)
        {
            ApiResult<bool>? result = await _userService.RoleAssign(Id, request);
            await _hubContext.Clients.All.SendAsync("listuser", result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            ApiResult<UserGetId>? user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpGet("GetListRole")]
        public async Task<IActionResult> GetListRole()
        {
            return Ok(await _userService.GetListRole());
        }

        [HttpPost("fotgotpassword")]
        public async Task<IActionResult> ForgotPassword(ChangePasswordRequest request)
        {
            return Ok(await _userService.ForgotPassword(request));
        }
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UserUpdateStatus request)
        {
            return Ok(await _userService.UpdateStatus(request));
        }

        [HttpGet("lookout/{Id}")]
        public async Task<IActionResult> LookOut(Guid Id)
        {
            var result = await _userService.LookUser(Id);
            if(result.IsSuccessed==true)
            {
                await _hubContext.Clients.All.SendAsync("lookout", Id);
                return Ok(result);
            }
            return Ok(result.Message);
        }
       
    }
}
