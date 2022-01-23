using AutoMapper;
using CRM_Management_Student.Backend.Data;
using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.ViewModels.Classes;
using CRM_Management_Student.Backend.ViewModels.Common;
using CRM_Management_Student.Backend.ViewModels.Subjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CRM_Management_Student.Backend.Application
{
    public interface ISubjectService
    {
        Task<PagedResultDto<SubjectVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<ApiResult<SubjectVm>> CreateAsync(SubjectCreate request);
        Task<ApiResult<bool>> DeleteAsync(Guid Id);
        Task<ApiResult<SubjectVm>> GetClassById(Guid Id);
        Task<ApiResult<SubjectVm>> UpdateAsync(Guid Id, SubjectUpdate request);
    }
    public class SubjectService : ISubjectService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public SubjectService(AppDbContext appDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }
        public async Task<ApiResult<SubjectVm>> CreateAsync(SubjectCreate request)
        {
            var result = await _appDbContext.Subjects.AddAsync(_mapper.Map<Subject>(request));
            return new ApiSuccessResult<SubjectVm>(_mapper.Map<SubjectVm>(result));
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

        public async Task<PagedResultDto<SubjectVm>> GetListAsync(PagedAndSortedResultRequestDto request)
        {
            var query = _appDbContext.Subjects.Include(x => x.SubjectClasses)
                                                     .ThenInclude(x => x.Class)
                                                     .AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(x => x.Name.Contains(request.Filter));
            }
            var result = await query.OrderBy(x => x.CreateDate).Skip((request.SkipCount - 1) * request.MaxResultCount)
                .Take(request.MaxResultCount)
                .ToListAsync();
            return new PagedResultDto<SubjectVm>
            {
                Items = _mapper.Map<List<SubjectVm>>(result),
                TotalCount = query.Count()
            };
        }

        public async Task<ApiResult<SubjectVm>> GetClassById(Guid Id)
        {
            var @class = await _appDbContext.Classes.Include(x => x.UserClasses)
                                                    .ThenInclude(x => x.AppUser)
                                                    .FirstOrDefaultAsync(x => x.Id.Equals(Id));
            if (@class != null)
            {
                return new ApiSuccessResult<SubjectVm>(_mapper.Map<SubjectVm>(@class));
            }
            return new ApiErrorResult<SubjectVm>("Can't find class");
        }

        public async Task<ApiResult<SubjectVm>> UpdateAsync(Guid Id, SubjectUpdate request)
        {
            var subject = await _appDbContext.Subjects.FindAsync(Id);
            if (subject == null) return new ApiErrorResult<SubjectVm>("Can't find subject");
            if (request.Description != null) subject.Description = request.Description;
            if (request.Name != null) subject.Name = request.Name;
            subject.UserModified = request.UserModified;
            _appDbContext.Subjects.Update(subject);
            await _appDbContext.SaveChangesAsync();
            return new ApiSuccessResult<SubjectVm>(_mapper.Map<SubjectVm>(subject));
        }
    }
}
