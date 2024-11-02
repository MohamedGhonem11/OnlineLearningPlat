using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineLearning.Entity.Entities;
using OnlineLearning.Infrastructure.Data;
using OnlineLearning.Service.Interfaces;
using OnlineLearning.Service.ViewModels;

namespace OnlineLearning.Infrastructure.Repositories
{
    public class ProgressTrackingService : IProgressTrackingService
    {
        private readonly ApplicationDbContext _context;

        public ProgressTrackingService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to calculate progress for an enrollment
        public async Task<int> CalculateProgress(int enrollmentId)
        {
            var totalAssignments = await _context.Assignments
                .Where(a => a.CourseId == (from e in _context.Enrollments where e.Id == enrollmentId select e.CourseId).FirstOrDefault())
                .CountAsync();

            var completedAssignments = await _context.ProgressTrackings
                .CountAsync(p => p.EnrollmentId == enrollmentId && p.IsCompleted);

            if (totalAssignments == 0) return 0;

            return (completedAssignments * 100) / totalAssignments;
        }

        // Method to fetch progress data for a specific course
        public async Task<List<ProgressTrackingViewModel>> GetCourseProgressAsync(int courseId)
        {
            var progressData = await _context.ProgressTrackings
                .Where(p => p.CourseId == courseId)
                .GroupBy(p => p.UserId)
                .Select(g => new ProgressTrackingViewModel
                {
                    Id = g.First().Id, // Ensure Id is set correctly for each progress
                    UserId = g.Key,
                    UserName = g.First().User.FullName,
                    ProgressPercentage = g.Average(p => p.CompletionPercentage),
                    IsCompleted = g.All(p => p.IsCompleted),
                    CourseId = courseId
                })
                .ToListAsync();

            return progressData;
        }







        public async Task<List<CourseEnrollmentViewModel>> GetCoursesWithEnrollmentCountAsync(string instructorId)
        {
            var courses = await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Select(c => new CourseEnrollmentViewModel
                {
                    CourseId = c.Id,
                    CourseTitle = c.Title,
                    StudentCount = c.Enrollments.Count
                })
                .ToListAsync();

            return courses;
        }

        public async Task InitializeProgressTracking(int courseId)
        {
            var enrollments = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Include(e => e.User) 
                .ToListAsync();

            var assignments = await _context.Assignments
                .Where(a => a.CourseId == courseId)
                .ToListAsync();

            // Create progress tracking for each student and each assignment ONLY IF it doesn't already exist
            foreach (var enrollment in enrollments)
            {
                foreach (var assignment in assignments)
                {
                    // Check if a progress entry already exists for this student and assignment
                    var existingProgress = await _context.ProgressTrackings
                        .AnyAsync(p => p.EnrollmentId == enrollment.Id && p.AssignmentId == assignment.Id);

                    if (!existingProgress)
                    {
                        var progressTracking = new ProgressTracking
                        {
                            EnrollmentId = enrollment.Id,
                            AssignmentId = assignment.Id,
                            CourseId = courseId,
                            IsCompleted = false,
                            CompletionPercentage = 0,
                            UserId = enrollment.UserId // Make sure UserId is assigned here
                        };

                        _context.ProgressTrackings.Add(progressTracking);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }




        //public async Task<List<ProgressTrackingViewModel>> GetAssignmentsForStudentAsync(int courseId, string userId)
        //{
        //    var assignments = await _context.ProgressTrackings
        //        .Where(p => p.CourseId == courseId && p.UserId == userId)
        //        .Include(p => p.Assignment) // Include assignment to get assignment details
        //        .Select(p => new ProgressTrackingViewModel
        //        {
        //            Id = p.Id,
        //            UserName = p.User.UserName,
        //            AssignmentTitle = p.Assignment.Title,  // Fetching assignment title if needed
        //            IsCompleted = p.IsCompleted,
        //            ProgressPercentage = p.CompletionPercentage,
        //            CourseId = p.CourseId
        //        })
        //        .ToListAsync();

        //    return assignments;
        //}

        public async Task<List<AssignmentProgressViewModel>> GetAssignmentsForStudentAsync(int courseId, string userId)
        {
            var assignments = await _context.ProgressTrackings
                .Where(p => p.CourseId == courseId && p.UserId == userId)
                .Select(p => new AssignmentProgressViewModel
                {
                    ProgressId = p.Id, 
                    CourseId = p.CourseId, 
                    UserId = p.UserId, 

                    AssignmentTitle = p.Assignment.Title,
                    Description = p.Assignment.Description,
                    IsCompleted = p.IsCompleted
                })
                .ToListAsync();

            return assignments;
        }









        // Method to update the progress for a specific student
        public async Task UpdateProgressAsync(int progressId, bool isCompleted)
        {
            var progress = await _context.ProgressTrackings.FindAsync(progressId);
            if (progress != null)
            {
                progress.IsCompleted = isCompleted;
                _context.ProgressTrackings.Update(progress);
                await _context.SaveChangesAsync();

                // Recalculate the course progress for the student
                var enrollmentId = progress.EnrollmentId;
                var totalAssignments = await _context.ProgressTrackings
                    .Where(p => p.EnrollmentId == enrollmentId)
                    .CountAsync();

                var completedAssignments = await _context.ProgressTrackings
                    .Where(p => p.EnrollmentId == enrollmentId && p.IsCompleted)
                    .CountAsync();

                var courseProgressEntries = await _context.ProgressTrackings
                    .Where(p => p.EnrollmentId == enrollmentId)
                    .ToListAsync();

                double completionPercentage = totalAssignments == 0 ? 0 : Math.Round((double)completedAssignments * 100 / totalAssignments, 2);

                foreach (var entry in courseProgressEntries)
                {
                    entry.CompletionPercentage = completionPercentage;
                    _context.ProgressTrackings.Update(entry);
                }

                await _context.SaveChangesAsync();
            }
        }




    }
}
