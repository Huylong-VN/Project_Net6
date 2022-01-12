namespace CRM_Management_Student.Backend.Data.Entity
{
    public class Subject : BaseEntity
    {
        public string? Name { set;get; }
        public string? Description { set;get; }
        public List<UserSubject>? UserSubjects { get; set; }
    }
}
