namespace CRM_Management_Student.Backend.ViewModels.Messages
{
    public class MessageSend
    {
        public Guid SendTo { set; get; }
        public string? Content { set; get; }
        public string? UserCreated { get; set; }
        public bool? Status { set; get; } = false;
    }
}
