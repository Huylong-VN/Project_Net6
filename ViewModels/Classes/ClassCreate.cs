namespace CRM_Management_Student.Backend.ViewModels.Classes
{
    public class ClassCreate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { set; get; }

        public string? UserCreated { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
