namespace CRM_Management_Student.Backend.Data.Entity
{
    public class ClassSubject
    {
        public Class? Class { get; set; }
        public Guid ClassId { set; get; }
        public Subject? Subject { get; set; }
        public Guid SubjectId { set; get; }

        public string? Score { set; get; }
    }
}
