using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Service.ViewModels
{
    public class ProgressTrackingViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public double ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }

        public int CourseId { get; set; }  // This will hold the course ID

        public string UserId { get; set; }  // Add this so we can pass it to the controller when linking student assignments


        public string AssignmentTitle { get; set; }  // Add this property if we need to display assignment titles


    }
}
