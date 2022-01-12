namespace CRM_Management_Student.Backend.Data.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string? UserCreated { get; set; }
        public string? UserModified { get; set; }
        public DateTime CreateDate { get; set; }=DateTime.Now;
        public DateTime ModifiedDate { get; set; }
    }
}
