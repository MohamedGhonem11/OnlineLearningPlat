using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Entity.Entities
{
    public class Course
    {
        public int Id { get; set; }  
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string InstructorId { get; set; }  
        public ApplicationUser Instructor { get; set; }  

        // Relationships
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Assignment> Assignments { get; set; }

        public ICollection<ProgressTracking> ProgressTrackings { get; set; }  

    }
}


