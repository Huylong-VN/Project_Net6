using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.ViewModels.Users;

namespace CRM_Management_Student.Backend.ViewModels.Classes
{
    public class ClassVm:BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public UserVm? AppUser { set; get; }
    }
}
