using Microsoft.AspNetCore.Identity;

namespace CRM_Management_Student.Backend.Data.Entity
{
    public class AppUserRole: IdentityUserRole<Guid>
    {
        public AppUser AppUser { get; set; }
        public AppRole AppRole { get; set; }
    }
}
