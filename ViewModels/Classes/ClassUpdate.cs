namespace CRM_Management_Student.Backend.ViewModels.Classes
{
    public class ClassUpdate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? UserModified { get; set; }
        public IFormFile? Image { set; get; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
