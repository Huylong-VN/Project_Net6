namespace CRM_Management_Student.Backend.ViewModels.Subjects
{
    public class SubjectUpdate
    {
        public string? Name { set; get; }
        public string? Description { set; get; }
        public string? UserModified { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
