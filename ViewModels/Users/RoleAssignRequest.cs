using CRM_Management_Student.Backend.ViewModels.Common;

namespace CRM_Management_Student.Backend.ViewModels.Users
{
    public class RoleAssignRequest
    {
        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}