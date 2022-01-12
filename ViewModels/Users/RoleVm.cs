namespace CRM_Management_Student.Backend.ViewModels.Users
{
    public class RoleVm
    {
        public Guid Id { set; get; }
        public string? Name { set; get; }
        public List<UserVm>? Users { set; get; }
    }
}