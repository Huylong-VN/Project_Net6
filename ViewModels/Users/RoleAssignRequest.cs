using CRM_Management_Student.Backend.ViewModels.Common;

namespace CRM_Management_Student.Backend.ViewModels.Users
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; }
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}