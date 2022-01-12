namespace CRM_Management_Student.Backend.ViewModels.Users
{
    public class CreateUserDto
    {
        public string? FullName { set; get; }
        public string? UserName { set; get; }
        public string? Password { set; get; }
        public string? Email { set; get; }
        public IFormFile? Image { set; get; }
        public string? Roles { set; get; }
    }
}