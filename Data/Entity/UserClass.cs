namespace CRM_Management_Student.Backend.Data.Entity
{
    public class UserClass
    {
        public AppUser? AppUser { get; set; }
        public Guid UserId { get; set; }
        public Class? @Class { get; set; }
        public Guid? ClassId { set; get; }
    }
}
