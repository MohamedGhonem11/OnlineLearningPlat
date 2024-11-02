using OnlineLearning.Entity.Entities;
using OnlineLearning.Infrastructure.Data;
using OnlineLearning.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace OnlineLearning.Infrastructure.Repositories
{


    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext _context;

        public AssignmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsForCourse(int courseId)
        {
            return await _context.Assignments.Where(a => a.CourseId == courseId).ToListAsync();
        }

        public async Task AddAssignment(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<Assignment> GetAssignmentByIdAsync(int assignmentId)
        {
            return await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == assignmentId);
        }

        public async Task UpdateAssignmentAsync(Assignment assignment)
        {
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssignmentAsync(int assignmentId)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == assignmentId);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }

    }

}
