using Microsoft.AspNetCore.Identity;

namespace CRM_Management_Student.Backend.Data.Entity
{
    public class AppRole:IdentityRole<Guid>
    {
        public List<AppUserRole> AppUserRoles { get; set; }
    }
}
