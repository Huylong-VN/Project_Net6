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
            builder.Entity<UserClass>(x =>
            {
                x.ToTable("UserClasses");
                x.HasKey(x => new { x.UserId, x.ClassId });
            });
            builder.Entity<UserSubject>(x =>
            {
                x.ToTable("UserSubjects");
                x.HasKey(x => new { x.UserId, x.SubjectId });
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
        public DbSet<UserSubject> UserSubjects { get; set; }

    }
}
