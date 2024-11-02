using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLearning.Service.ViewModels
{
    public class CourseEnrollmentViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int StudentCount { get; set; }
    }
}
