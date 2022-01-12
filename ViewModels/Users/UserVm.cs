namespace CRM_Management_Student.Backend.ViewModels.Users
{
    public class UserVm
    {
        public Guid Id { set; get; }
        public string? UserName { set; get; }
        public string? FullName { set; get; }
        public string? Image { set; get; }
        public string? Email { set; get; }
        public bool? Status { set; get; }
        public string? PhoneNumber { set; get; }
        public RoleVm AppRole { set; get; }
    }
}