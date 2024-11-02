using OnlineLearning.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Service.Interfaces
{
    public interface IProgressTrackingService
    {
        Task<List<ProgressTrackingViewModel>> GetCourseProgressAsync(int courseId);
        Task UpdateProgressAsync(int progressId, bool isCompleted);
        Task<int> CalculateProgress(int enrollmentId); 

        Task InitializeProgressTracking(int courseId);

        Task<List<AssignmentProgressViewModel>> GetAssignmentsForStudentAsync(int courseId, string userId); // New method to fetch assignments for a student


        //Task<List<CourseEnrollmentViewModel>> GetCoursesWithEnrollmentCountAsync(string instructorId);

    }

}
