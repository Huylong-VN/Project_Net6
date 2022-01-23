namespace CRM_Management_Student.Backend.Data.Entity
{
    public class Class:BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public List<UserClass>? UserClasses { get; set; }
        public List<ClassSubject>? SubjectClasses { get; set; }

    }
}
