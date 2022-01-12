namespace CRM_Management_Student.Backend.ViewModels.Notifications
{
    public class NotificationVm
    {
        public Guid Id { get; set; }
        public string? Title { set; get; }
        public string? Message { set; get; }
        public string? UserCreated { get; set; }
        public string? UserModified { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
