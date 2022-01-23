namespace CRM_Management_Student.Backend.Data.Entity
{
    public class Message:BaseEntity
    {
        public string? Content { set; get; }
        public AppUser? AppUser { set; get; }
        public Guid UserId { set; get; }
        public int? CountNewMessage { set; get; }
        public bool? Status { set; get; }
    }
}
