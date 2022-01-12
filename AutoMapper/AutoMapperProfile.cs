using AutoMapper;
using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.ViewModels.Classes;
using CRM_Management_Student.Backend.ViewModels.Notifications;
using CRM_Management_Student.Backend.ViewModels.Subjects;
using CRM_Management_Student.Backend.ViewModels.Users;

namespace CRM_Management_Student.Backend.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, UserVm>().ReverseMap();
            CreateMap<AppUser, UserCourseVm>().ReverseMap();
            CreateMap<AppUser, CreateUserDto>().ReverseMap();
            CreateMap<AppUser, UpdateUserDto>().ReverseMap();
            CreateMap<AppRole, RoleVm>().ReverseMap();
            CreateMap<Notification, NotificationVm>().ReverseMap();
            CreateMap<Notification, NotificationCreate>().ReverseMap();
            CreateMap<Class, ClassVm>().ReverseMap();
            CreateMap<Subject, SubjectVm>().ReverseMap();
        }
    }
}