using Microsoft.EntityFrameworkCore;
using OnlineLearning.Entity.Entities;
using OnlineLearning.Infrastructure.Data;
using OnlineLearning.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineLearning.Infrastructure.Repositories
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EnrollUserInCourse(string userId, int courseId)
        {
            // Check if the user is already enrolled
            if (await _context.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId))
                return false;

            // Create new enrollment
            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrollmentDate = DateTime.UtcNow
            };

            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<int>> GetUserEnrolledCourseIdsAsync(string userId)
        {
            return await _context.Enrollments
                .Where(e => e.UserId == userId)
                .Select(e => e.CourseId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Course>> GetUserEnrolledCoursesAsync(string userId)
        {
            return await _context.Enrollments
                .Where(e => e.UserId == userId)
                .Include(e => e.Course) // Include course details
                .ThenInclude(c => c.Instructor) // Include the instructor details
                .Select(e => e.Course)
                .ToListAsync();
        }
    }
}
