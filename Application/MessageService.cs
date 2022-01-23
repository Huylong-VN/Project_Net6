using AutoMapper;
using CRM_Management_Student.Backend.Data;
using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.ViewModels.Common;
using CRM_Management_Student.Backend.ViewModels.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CRM_Management_Student.Backend.Application
{
    public interface IMessageService
    {
        Task<ApiResult<MessageVm>> SendToUser(MessageSend send);
        Task<ApiResult<bool>> UpdateStatus(Guid Id);
        Task<ApiResult<bool>> Delete(Guid Id);

        Task<PagedResultDto<MessageVm>> GetListMessageWithUserId(string Username,PagedAndSortedResultRequestDto request);
    }
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public MessageService(AppDbContext dbContext, UserManager<AppUser> userManager, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<ApiResult<bool>> Delete(Guid Id)
        {
            var message = await _dbContext.Messages.FindAsync(Id);
            if (message == null) return new ApiErrorResult<bool>("can't find message");
            _dbContext.Messages.Remove(message);
            await _dbContext.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<PagedResultDto<MessageVm>> GetListMessageWithUserId(string Username, PagedAndSortedResultRequestDto request)
        {
            var message = _dbContext.Messages.Include(x => x.AppUser).Where(x=>x.UserId.Equals(Username)).ToList();
            return new PagedResultDto<MessageVm>
            {
                Items=_mapper.Map<List<MessageVm>>(message),
                TotalCount=message.Count()
            };
        }

        public async Task<ApiResult<MessageVm>> SendToUser(MessageSend send)
        {
            var message = await _dbContext.Messages.AddAsync(_mapper.Map<Message>(send));
            await _dbContext.SaveChangesAsync();
            return new ApiSuccessResult<MessageVm>(_mapper.Map<MessageVm>(message));
        }

        public async Task<ApiResult<bool>> UpdateStatus(Guid Id)
        {
            var message = await _dbContext.Messages.FindAsync(Id);
            if (message == null) return new ApiErrorResult<bool>("can't find message");
            message.Status = true;
            await _dbContext.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

    }
}
