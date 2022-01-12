﻿namespace CRM_Management_Student.Backend.ViewModels.Users
{
    public class UserCourseVm
    {
        public Guid? Id { set; get; }
        public string? UserName { set; get; }
        public string? FullName { set; get; }
        public string? Email { set; get; }
        public string? PhoneNumber { set; get; }
        public IList<string>? Roles { set; get; }
    }
}