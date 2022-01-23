using CRM_Management_Student.Backend.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM_Management_Student.Backend.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Message>(x =>
            {
                x.ToTable("Messages");
                x.HasKey(x => x.Id);
                x.HasOne(x => x.AppUser).WithMany(x => x.Messages).HasForeignKey(x => x.UserId);
            });
            builder.Entity<UserClass>(x =>
            {
                x.ToTable("UserClasses");
                x.HasKey(x => new { x.UserId, x.ClassId });
                x.HasOne(x => x.AppUser).WithMany(x => x.UserClasses).HasForeignKey(x => x.UserId);
                x.HasOne(x => x.Class).WithMany(x => x.UserClasses).HasForeignKey(x => x.ClassId);
            });
            builder.Entity<ClassSubject>(x =>
            {
                x.ToTable("SubjectClasses");
                x.HasKey(x => new { x.ClassId, x.SubjectId });
                x.HasOne(x => x.Class).WithMany(x => x.SubjectClasses).HasForeignKey(x => x.ClassId);
                x.HasOne(x => x.Subject).WithMany(x => x.SubjectClasses).HasForeignKey(x => x.ClassId);
            });
            builder.Entity<Class>(x =>
            {
                x.HasKey(x => x.Id);
            });
            builder.Entity<Subject>(x =>
            {
                x.HasKey(x => x.Id);
            });




            //User
            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>().HasData(new AppUser
            {
                Id = new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672"),
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "huynabhaf190133@fpt.edu.vn",
                NormalizedEmail = "huynabhaf190133@fpt.edu.vn",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "1"),
                SecurityStamp = string.Empty,
                FullName = "Nguyen Anh Huy",
                PhoneNumber = "0399056507",
            });
            builder.Entity<AppRole>().HasData(new AppRole()
            {
                Id = new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                Name = "admin",
                NormalizedName = "admin",
            });
            builder.Entity<AppRole>().HasData(new AppRole()
            {
                Id = new Guid("5e54e16d-681f-4388-bf83-5cc4a57c29cd"),
                Name = "student",
                NormalizedName = "student",
            });

            builder.Entity<AppUserRole>().HasData(new AppUserRole()
            {
                RoleId = new Guid("cc88ab6f-5d66-4c30-a60e-8f5254f1e112"),
                UserId = new Guid("0027068e-4c5d-4ecb-a157-b9cc063cd672")
            });

            //IdentityUserLogin
            builder.Entity<IdentityUserLogin<Guid>>().HasKey(x => x.UserId);
            //IdentityUserRole
            builder.Entity<AppUserRole>(x =>
            {
                x.HasKey(x => new { x.UserId, x.RoleId });
                x.HasOne(x => x.AppRole).WithMany(x => x.AppUserRoles).HasForeignKey(x => x.RoleId);
                x.HasOne(x => x.AppUser).WithMany(x => x.AppUserRoles).HasForeignKey(x => x.UserId);
            });
            //IdentityUserToken
            builder.Entity<IdentityUserToken<Guid>>().HasKey(x => x.UserId);

            //
            builder.Entity<Notification>().HasKey(x => x.Id);
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<UserClass> UserClasses { get; set; }
        public DbSet<ClassSubject> UserSubjects { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
