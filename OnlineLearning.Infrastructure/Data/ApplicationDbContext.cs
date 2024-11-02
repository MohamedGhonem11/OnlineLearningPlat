using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLearning.Entity.Entities;
using System.Reflection;

namespace OnlineLearning.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ProgressTracking> ProgressTrackings { get; set; }

        // Add this line to include Assignments
        public DbSet<Assignment> Assignments { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<ProgressTracking>()
          .HasOne(pt => pt.Assignment)
          .WithMany(a => a.ProgressTrackings)
          .HasForeignKey(pt => pt.AssignmentId)
          .OnDelete(DeleteBehavior.Cascade);
            SeedRoles(builder);
            base.OnModelCreating(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { ConcurrencyStamp = "1", Name = "Student", NormalizedName = "STUDENT" },
                new IdentityRole { ConcurrencyStamp = "2", Name = "Instructor", NormalizedName = "INSTRUCTOR" },
                new IdentityRole { ConcurrencyStamp = "3", Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }

}
