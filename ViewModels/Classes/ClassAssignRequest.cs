using CRM_Management_Student.Backend.ViewModels.Common;

namespace CRM_Management_Student.Backend.ViewModels.Classes
{
    public class ClassAssignRequest
    {
        public List<SelectItem> Users { get; set; } = new List<SelectItem>();
    }
}
