using AutoMapper;
using CRM_Management_Student.Backend.Data;
using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.ViewModels.Classes;
using CRM_Management_Student.Backend.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http.Headers;

namespace CRM_Management_Student.Backend.Application
{
    public interface IClassService
    {
        Task<PagedResultDto<ClassVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<ApiResult<ClassVm>> CreateAsync(ClassCreate request);
        Task<ApiResult<bool>> DeleteAsync(Guid Id);
        Task<ApiResult<ClassVm>> GetClassById(Guid Id);
        Task<ApiResult<ClassVm>> UpdateAsync(Guid Id,ClassUpdate request);
        Task<ApiResult<bool>> AddNewStudent(Guid Id,ClassAssignRequest request);
    }
    public class ClassService : IClassService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;

        public ClassService(AppDbContext appDbContext,IMapper mapper, IStorageService storageService)
        {
            _storageService = storageService;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<ApiResult<ClassVm>> CreateAsync(ClassCreate request)
        {
            var @class = _mapper.Map<Class>(request);
            if (request.Image != null) @class.Image = await SaveFile(request.Image);
            var result=await _appDbContext.Classes.AddAsync(@class);
            await _appDbContext.SaveChangesAsync();
            return new ApiSuccessResult<ClassVm>(_mapper.Map<ClassVm>(result.Entity));
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid Id)
        {
            var @class = await _appDbContext.Classes.FindAsync(Id);
            if (@class != null)
            {
                if (@class.Image != "")
                {
                    await _storageService.DeleteFileAsync(@class.Image);
                }
                _appDbContext.Classes.Remove(@class);
                await _appDbContext.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Can't find class");
        }

        public async Task<PagedResultDto<ClassVm>> GetListAsync(PagedAndSortedResultRequestDto request)
        {
            var query = _appDbContext.Classes.Include(x => x.UserClasses)
                                                     .ThenInclude(x => x.AppUser)
                                                     .AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(x => x.Name.Contains(request.Filter));
            }
            var result = await query.OrderBy(x=>x.CreateDate).Skip((request.SkipCount - 1) * request.MaxResultCount)
                .Take(request.MaxResultCount)
                .ToListAsync();
            return new PagedResultDto<ClassVm>
            {
                Items = _mapper.Map<List<ClassVm>>(result),
                TotalCount = query.Count()
            };
        }

        public async Task<ApiResult<ClassVm>> GetClassById(Guid Id)
        {
            var @class =await _appDbContext.Classes.Include(x => x.UserClasses)
                                                    .ThenInclude(x => x.AppUser)
                                                    .ThenInclude(x=>x.AppUserRoles).ThenInclude(x=>x.AppRole)
                                                    .FirstOrDefaultAsync(x=>x.Id.Equals(Id));
            if (@class != null)
            {
                return new ApiSuccessResult<ClassVm>(_mapper.Map<ClassVm>(@class));
            }
            return new ApiErrorResult<ClassVm>("Can't find class");
        }

        public async Task<ApiResult<ClassVm>> UpdateAsync(Guid Id, ClassUpdate request)
        {
            var @class = await _appDbContext.Classes.FindAsync(Id);
            if (@class == null) return new ApiErrorResult<ClassVm>("Can't find class");
            if (@class.Image != "")
            {
                await _storageService.DeleteFileAsync(@class.Image);
            }
            if (request.Description!=null) @class.Description = request.Description; 
            if(request.Name!=null) @class.Name = request.Name;
            if(request.Image!=null) @class.Image = await SaveFile(request.Image);
            @class.UserModified=request.UserModified;
            _appDbContext.Classes.Update(@class);
            await _appDbContext.SaveChangesAsync();
            return new ApiSuccessResult<ClassVm>(_mapper.Map<ClassVm>(@class));
        }

        public async Task<ApiResult<bool>> AddNewStudent(Guid Id, ClassAssignRequest request)
        {
            var @class = await _appDbContext.Classes.FindAsync(Id);
            if (@class == null)
            {
                return new ApiErrorResult<bool>("Class không tồn tại");
            }
            // Tìm ra list học sinh sẽ bị xóa
            var removed = request.Users.Where(x => x.Selected == false).Select(x => x.Id).ToList();
            foreach (var id in removed)
            {
                var UserClass = await _appDbContext.UserClasses.FindAsync(new Guid(id), Id);
                if (UserClass != null)
                {
                    _appDbContext.UserClasses.Remove(UserClass);
                }
            }
            // Tìm ra list học sinh sẽ được thêm vào
            var added = request.Users.Where(x => x.Selected == true).Select(x => x.Id).ToList();
            foreach (var id in added)
            {
                var userId = new Guid(id);
                var UserClass = await _appDbContext.UserClasses.FindAsync(userId, Id);
                if (UserClass == null)
                {
                    await _appDbContext.UserClasses.AddAsync(new UserClass()
                    {
                        ClassId = Id,
                        UserId= userId
                    });
                }
            }
            await _appDbContext.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}
