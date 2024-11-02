using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearning.Entity.Entities;

namespace OnlineLearning.Infrastructure.Config
{
    public class ProgressTrackingConfig : IEntityTypeConfiguration<ProgressTracking>
    {
        public void Configure(EntityTypeBuilder<ProgressTracking> builder)
        {
            // Primary Key
            builder.HasKey(p => p.Id);

            // Relationship with Enrollment
            builder.HasOne(p => p.Enrollment)
                   .WithMany(e => e.ProgressTrackings)
                   .HasForeignKey(p => p.EnrollmentId)
                   .OnDelete(DeleteBehavior.Restrict);  // No cascading delete for Enrollment

            // Relationship with Assignment
            builder.HasOne(p => p.Assignment)
                   .WithMany(a => a.ProgressTrackings)
                   .HasForeignKey(p => p.AssignmentId)
                   .OnDelete(DeleteBehavior.Restrict);  // No cascading delete for Assignment

            // Relationship with Course
            builder.HasOne(p => p.Course)
                   .WithMany(c => c.ProgressTrackings)
                   .HasForeignKey(p => p.CourseId)
                   .OnDelete(DeleteBehavior.NoAction);  // No action on delete for Course

            // Relationship with User
            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.NoAction);  // No action on delete for User
        }
    }
}
