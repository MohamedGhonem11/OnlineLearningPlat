using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Service.ViewModels
{
    public class AssignmentProgressViewModel
    {
        public int ProgressId { get; set; } // Unique ID for the progress tracking entry
        public int CourseId { get; set; } // ID of the course
        public string UserId { get; set; } // ID of the student

        public string AssignmentTitle { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }


}
