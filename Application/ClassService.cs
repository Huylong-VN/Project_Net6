using AutoMapper;
using CRM_Management_Student.Backend.Data;
using CRM_Management_Student.Backend.ViewModels.Classes;
using CRM_Management_Student.Backend.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CRM_Management_Student.Backend.Application
{
    public interface IClassService
    {
        Task<PagedResultDto<ClassVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<ApiResult<ClassVm>> CreateAsync(ClassCreate request);
        Task<ApiResult<bool>> DeleteAsync(Guid Id);
        Task<ApiResult<ClassVm>> GetClassById(Guid Id);
    }
    public class ClassService : IClassService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public ClassService(AppDbContext appDbContext,IMapper mapper)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }
        public Task<ApiResult<ClassVm>> CreateAsync(ClassCreate request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid Id)
        {
            var @class = await _appDbContext.Classes.FindAsync(Id);
            if (@class != null)
            {
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
                                                    .FirstOrDefaultAsync(x=>x.Id.Equals(Id));
            if (@class != null)
            {
                return new ApiSuccessResult<ClassVm>(_mapper.Map<ClassVm>(@class));
            }
            return new ApiErrorResult<ClassVm>("Can't find class");
        }
    }
}
