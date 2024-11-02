using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Entity.Entities
{
    public class ProgressTracking
    {
        public int Id { get; set; }  // Primary key

        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }

        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        public bool IsCompleted { get; set; }

        public double CompletionPercentage { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }  // Navigation property for User


    }
}

