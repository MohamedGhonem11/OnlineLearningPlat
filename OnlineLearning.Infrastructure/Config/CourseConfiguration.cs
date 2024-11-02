using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearning.Entity.Entities;

namespace OnlineLearning.Infrastructure.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Primary Key
            builder.HasKey(c => c.Id);  // Use Id, as CourseId is unnecessary

            // Title validation
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            // Description validation
            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            // Start and End Date validation
            builder.Property(c => c.StartDate)
                .IsRequired();

            builder.Property(c => c.EndDate)
                .IsRequired();

            // Relationships
            builder.HasOne(c => c.Instructor)
                .WithMany(u => u.CoursesTaught)  // Instructor teaches multiple courses
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            builder.HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascades delete if course is deleted
        }
    }
}
