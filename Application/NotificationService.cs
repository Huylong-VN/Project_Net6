using AutoMapper;
using CRM_Management_Student.Backend.Data;
using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.ViewModels.Common;
using CRM_Management_Student.Backend.ViewModels.Notifications;
using Microsoft.EntityFrameworkCore;

namespace CRM_Management_Student.Backend.Application
{
    public interface INotificationService
    {
        Task<PagedResultDto<NotificationVm>> GetListAsync(PagedAndSortedResultRequestDto request);

        Task<ApiResult<NotificationVm>> CreateAsync(NotificationCreate request);
        Task<ApiResult<bool>> DeleteAsync(Guid Id);
        Task<ApiResult<NotificationVm>> GetNotifyById(Guid Id);
    }
    public class NotificationService:INotificationService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public NotificationService(AppDbContext appDbContext,IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<ApiResult<NotificationVm>> CreateAsync(NotificationCreate request)
        {
            var notify = _mapper.Map<Notification>(request);
            var result=await _appDbContext.Notifications.AddAsync(notify);
            await _appDbContext.SaveChangesAsync();
            return new ApiSuccessResult<NotificationVm>(_mapper.Map<NotificationVm>(result.Entity));
        }

        public async Task<ApiResult<bool>> DeleteAsync(Guid Id)
        {
            var notify = await _appDbContext.Notifications.FindAsync(Id);
            if (notify != null)
            {
                _appDbContext.Notifications.Remove(notify);
                await _appDbContext.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Can't find notify");
        }

        public async Task<PagedResultDto<NotificationVm>> GetListAsync(PagedAndSortedResultRequestDto request)
        {
            var query = _appDbContext.Notifications;

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query.Where(x => x.Title.Contains(request.Filter) || x.Message.Contains(request.Filter));
            }
            if (string.IsNullOrEmpty(request.Sorting)) request.Sorting = nameof(Notification.Title);
            if (request.SkipCount == 0) request.SkipCount = 1;
            if (request.MaxResultCount == 0) request.MaxResultCount = 10;
            var t = await query.OrderBy(x => x.Id).Skip((request.SkipCount - 1) * request.MaxResultCount).
                Take(request.MaxResultCount).ToListAsync();
            return new PagedResultDto<NotificationVm> { Items = _mapper.Map<List<NotificationVm>>(t), TotalCount = query.Count() };
        }

        public async Task<ApiResult<NotificationVm>> GetNotifyById(Guid Id)
        {
            var notify = await _appDbContext.Notifications.FindAsync(Id);
            if (notify == null) return new ApiErrorResult<NotificationVm>("Can't find notify");
            return new ApiSuccessResult<NotificationVm>(_mapper.Map<NotificationVm>(notify));
        }
    }
}
