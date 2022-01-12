namespace CRM_Management_Student.Backend.ViewModels.Users
{
    public class UpdateUserDto
    {
        public string? FullName { set; get; }
        public string? Address { set; get; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }

        public IFormFile? Image { set; get; }
    }
}