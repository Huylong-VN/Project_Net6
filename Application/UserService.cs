using AutoMapper;
using CRM_Management_Student.Backend.Data;
using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.ViewModels.Common;
using CRM_Management_Student.Backend.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace CRM_Management_Student.Backend.Application
{
    public interface IUserService
    {
        Task<ApiResult<UserAuthenticate>> Authenticate(UserLoginRequest request);

        Task<PagedResultDto<UserVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<ApiResult<UserVm>> CreateAsync(CreateUserDto request);

        Task<ApiResult<UserVm>> UpdateAsync(Guid Id, UpdateUserDto request);

        Task<bool> DeleteAsync(Guid Id);

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);

        Task<ApiResult<UserGetId>> GetById(Guid id);

        Task<List<RoleVm>> GetListRole();

        Task<ApiResult<UserVm>> ForgotPassword(ChangePasswordRequest request);
        Task<ApiResult<bool>> UpdateStatus(UserUpdateStatus request);

        Task<ApiResult<bool>> LookUser(Guid Id);



    }
    public class UserService:IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        public readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly IStorageService _storageService;

        public UserService(RoleManager<AppRole> roleManager, IConfiguration configuration,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, AppDbContext appDbContext,
            IStorageService storageService)
        {
            _storageService= storageService;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
            _mapper = mapper;
            _signInManager = signInManager;
            _configuration = configuration;
            _userManager = userManager;
        }
        private string GenerateToken(AppUser user)
        {
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["JwtConfig:Issuer"],
            _configuration["JwtConfig:Audience"], claims, expires: DateTime.Now.AddHours(3), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiResult<UserAuthenticate>> Authenticate(UserLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<UserAuthenticate>("Can't find user in system");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);
            if (!result.Succeeded) return new ApiErrorResult<UserAuthenticate>("Password Wrong");
            var roles = await _userManager.GetRolesAsync(user);
            return new ApiSuccessResult<UserAuthenticate>(new UserAuthenticate
            {
                AccessToken = GenerateToken(user),
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Roles = roles,
                Image=user.Image,
            }
            );
        }
        private async Task<string> SaveFile(IFormFile file)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<ApiResult<UserVm>> CreateAsync(CreateUserDto request)
        {
            
            if (await _userManager.FindByEmailAsync(request.Email) != null) return new ApiErrorResult<UserVm>("Email Already exist");
            if(await _userManager.FindByNameAsync(request.UserName) != null) return new ApiErrorResult<UserVm>("UserName Already exist");
            var user = new AppUser()
            {
                Email = request.Email,
                UserName = request.UserName,
                FullName=request.FullName,
            };
            if(request.Image != null)
            {
                user.Image = await SaveFile(request.Image);
            }
            var result=await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return new ApiErrorResult<UserVm>("Password must Have A-Z and Have UpperCase and number");
            await _userManager.AddToRoleAsync(user, request.Roles);
            return new ApiSuccessResult<UserVm>(_mapper.Map<UserVm>(user));
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null) return false;
            if (user.Image != null)
            {
                await _storageService.DeleteFileAsync(user.Image);
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<PagedResultDto<UserVm>> GetListAsync(PagedAndSortedResultRequestDto request)
        {
            var query = _userManager.Users.Include(x => x.AppUserRoles)
                .ThenInclude(x => x.AppRole).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query=query.Where(x => x.UserName.Contains(request.Filter)|| x.FullName.Contains(request.Filter)|| x.Email.Contains(request.Filter) || x.AppUserRoles.Any(r=>r.AppRole.Name.Equals(request.Filter)));
            }
            if (string.IsNullOrEmpty(request.Sorting)) request.Sorting = nameof(AppUser.UserName);
            if (request.SkipCount == 0) request.SkipCount = 1;
            if (request.MaxResultCount == 0) request.MaxResultCount = 10;
            var t = await query.OrderBy(x => x.Id).Skip((request.SkipCount - 1) * request.MaxResultCount).
                Take(request.MaxResultCount).ToListAsync();
            return new PagedResultDto<UserVm> { Items = _mapper.Map<List<UserVm>>(t), TotalCount = query.Count() };
        }

        public async Task<ApiResult<UserVm>> UpdateAsync(Guid Id, UpdateUserDto request)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null) return new ApiErrorResult<UserVm>("Can't find this account");
            if (user.Image != null)
            {
                await _storageService.DeleteFileAsync(user.Image);
            }
            if (request.FullName != null) user.FullName = request.FullName;
            if (request.Email != null) user.Email = request.Email;
            if (request.PhoneNumber!=null) user.PhoneNumber = request.PhoneNumber;
            if (request.Image != null) user.Image = await SaveFile(request.Image);

            await _userManager.UpdateAsync(user);

            return new ApiSuccessResult<UserVm>(_mapper.Map<UserVm>(user));
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<UserGetId>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserGetId>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = new UserGetId()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
                UserName = user.UserName,
                Roles = roles,
                Status= user.Status,
                Image=user.Image
            };
            return new ApiSuccessResult<UserGetId>(userVm);
        }

        public async Task<List<RoleVm>> GetListRole()
        {
            var query = await _roleManager.Roles.Include(x => x.AppUserRoles).ThenInclude(x => x.AppUser).ToListAsync();
            var result = query.Select(x => new RoleVm
            {
                Users = _mapper.Map<List<UserVm>>(x.AppUserRoles.Select(x => x.AppUser)),
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return result;
        }

        public async Task<ApiResult<UserVm>> ForgotPassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return new ApiErrorResult<UserVm>("Can't find this user");
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<UserVm>();
            }
            return new ApiErrorResult<UserVm>("Current Password maybe wrong !!");
        }

        public async Task<ApiResult<bool>> UpdateStatus(UserUpdateStatus request)
        {
            if (request != null)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user != null)
                {
                    user.Status = request.Status;
                    await _userManager.UpdateAsync(user);
                    return new ApiSuccessResult<bool>();
                }
            }
            return new ApiErrorResult<bool>("Request Can't null");
        }

        public async Task<ApiResult<bool>> LookUser(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null) return new ApiErrorResult<bool>("can't find user");
            user.LockoutEnabled = true;
            await _userManager.SetLockoutEnabledAsync(user,true);
            await _userManager.UpdateAsync(user);
            return new ApiErrorResult<bool>();
        }

    }
}
