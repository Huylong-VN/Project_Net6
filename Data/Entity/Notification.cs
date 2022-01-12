namespace CRM_Management_Student.Backend.Data.Entity
{
    public class Notification:BaseEntity
    {
        public string? Title { set; get; }
        public string? Message { set; get; }
    }
}
