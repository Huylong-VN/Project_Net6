using Microsoft.AspNetCore.Identity;

namespace CRM_Management_Student.Backend.Data.Entity
{
    public class AppUser:IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? Image { set; get; }
        public bool? Status { set; get; } = false;
        public List<AppUserRole>? AppUserRoles { get; set; }
        public List<UserSubject>? UserSubjects { get; set; }
        public List<UserClass>? UserClasses { get; set; }
    }
}
