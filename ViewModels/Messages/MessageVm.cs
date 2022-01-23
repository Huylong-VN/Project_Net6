namespace CRM_Management_Student.Backend.ViewModels.Messages
{
    public class MessageVm
    {
        public Guid Id { get; set; }
        public string? UserCreated { get; set; }
        public string? UserModified { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? Content { set; get; }
        public Guid SendTo { set; get; }
        public int? CountNewMessage { set; get; }
        public bool? Status { set; get; }
    }
}
