using OnlineLearning.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;

namespace OnlineLearning.Service.Interfaces
{
    public interface IEnrollmentService
    {
        Task<bool> EnrollUserInCourse(string userId, int courseId);
        Task<IEnumerable<int>> GetUserEnrolledCourseIdsAsync(string userId);
        Task<IEnumerable<Course>> GetUserEnrolledCoursesAsync(string userId); // New method
    }
}
