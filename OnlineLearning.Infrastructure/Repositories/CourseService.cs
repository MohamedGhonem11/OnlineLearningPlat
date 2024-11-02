using Microsoft.EntityFrameworkCore;
using OnlineLearning.Entity.Entities;
using OnlineLearning.Infrastructure.Data;
using OnlineLearning.Service.Interfaces;
using OnlineLearning.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Infrastructure.Repositories
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.Include(c => c.Instructor).ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.Include(c => c.Instructor)
                                         .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
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


        public async Task AddAssignmentAsync(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Course>> FilterCoursesAsync(string searchTerm)
        {
            // Normalize the search term to lower case for case-insensitive search
            searchTerm = searchTerm?.ToLower();

            return await _context.Courses
                .Where(c => c.Title.ToLower().Contains(searchTerm) ||
                             c.Description.ToLower().Contains(searchTerm)) // Search in both Title and Description
                .ToListAsync();
        }
    }

}
