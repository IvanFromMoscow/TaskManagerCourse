using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ProjectAdmin> ProjectAdmins { get; set; }
        public DbSet<Project> Projects{ get; set; }
        public DbSet<Desk> Desks{ get; set; }
        public DbSet<Task> Tasks { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
            if (Users.Any(u => u.Status == UserStatus.Admin) == false)
            {
                var admin = new User( "Jhon", "Jhonson", "admin", "qwerty", UserStatus.Admin, "1234");
                Users.Add(admin);
                SaveChanges();
            }
        }

    }
}
