using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Entity.Entities
{
    public class Enrollment
    {
        public int Id { get; set; } 
        public int CourseId { get; set; }
        public Course Course { get; set; }  
        public string UserId { get; set; }  
        public ApplicationUser User { get; set; }  

        public DateTime EnrollmentDate { get; set; }  

        // Relationships
        public ICollection<ProgressTracking> ProgressTrackings { get; set; }
    }
}



