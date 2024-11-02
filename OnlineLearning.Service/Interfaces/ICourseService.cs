using OnlineLearning.Entity.Entities;
using OnlineLearning.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Service.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> FilterCoursesAsync(string title);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int id);

        Task AddAssignmentAsync(Assignment assignment);
        Task<List<CourseEnrollmentViewModel>> GetCoursesWithEnrollmentCountAsync(string instructorId);

    }

}
