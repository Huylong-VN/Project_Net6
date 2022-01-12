namespace CRM_Management_Student.Backend.Data.Entity
{
    public class UserSubject
    {
        public AppUser? AppUser { get; set; }
        public Guid UserId { set; get; }
        public Subject? Subject { get; set; }
        public Guid SubjectId { set; get; }

        public string? Score { set; get; }
    }
}
