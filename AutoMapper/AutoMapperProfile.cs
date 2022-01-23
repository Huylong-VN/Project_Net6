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
            CreateMap<AppUser, UserVm>().ForMember(x => x.AppRole, x => x.MapFrom(x => x.AppUserRoles.Select(x => x.AppRole))).ReverseMap();
            CreateMap<AppUser, UserCourseVm>().ReverseMap();
            CreateMap<AppUser, CreateUserDto>().ReverseMap();
            CreateMap<AppUser, UpdateUserDto>().ReverseMap();
            CreateMap<AppRole, RoleVm>().ReverseMap();
            CreateMap<Notification, NotificationVm>().ReverseMap();
            CreateMap<Notification, NotificationCreate>().ReverseMap();
            CreateMap<Class, ClassVm>().ForMember(x => x.AppUser, x => x.MapFrom(x => x.UserClasses.Select(x => x.AppUser))).ReverseMap();
            CreateMap<Class, ClassCreate>().ReverseMap();
            CreateMap<Class, ClassUpdate>().ReverseMap();
            CreateMap<Subject, SubjectVm>().ReverseMap();
            CreateMap<Subject, SubjectCreate>().ReverseMap();
            CreateMap<Subject, SubjectUpdate>().ReverseMap();
        }
    }
}