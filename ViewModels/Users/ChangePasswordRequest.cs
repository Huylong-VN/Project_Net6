namespace CRM_Management_Student.Backend.ViewModels.Users
{
    public class ChangePasswordRequest
    {
        public Guid Id { set; get; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { set; get; }
    }
}
