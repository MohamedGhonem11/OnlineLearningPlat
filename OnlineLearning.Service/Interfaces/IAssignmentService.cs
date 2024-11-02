using OnlineLearning.Entity.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Service.Interfaces
{
    public interface IAssignmentService
    {
        Task<IEnumerable<Assignment>> GetAssignmentsForCourse(int courseId);
        Task AddAssignment(Assignment assignment);
        Task<Assignment> GetAssignmentByIdAsync(int assignmentId);
        Task UpdateAssignmentAsync(Assignment assignment);
        Task DeleteAssignmentAsync(int assignmentId);
    }
}
