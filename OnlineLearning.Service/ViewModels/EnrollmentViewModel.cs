using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace OnlineLearning.Service.ViewModels
    {
        public class EnrollmentViewModel
        {
            public IEnumerable<CourseViewModel> Courses { get; set; }
            public IEnumerable<int> EnrolledCourseIds { get; set; }
        }
    }


