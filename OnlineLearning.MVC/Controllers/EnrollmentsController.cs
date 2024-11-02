using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearning.Service.Interfaces;
using OnlineLearning.Service.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineLearning.Web.Controllers
{
    [Authorize(Roles ="Student")]
    public class EnrollmentsController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(ICourseService courseService, IEnrollmentService enrollmentService)
        {
            _courseService = courseService;
            _enrollmentService = enrollmentService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var courses = string.IsNullOrWhiteSpace(searchString)
                ? await _courseService.GetAllCoursesAsync()
                : await _courseService.FilterCoursesAsync(searchString); // Ensure you have implemented FilterCoursesAsync

            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user's ID
            var result = await _enrollmentService.EnrollUserInCourse(userId, courseId);

            if (result)
            {
                TempData["SuccessMessage"] = "You have successfully enrolled in the course.";
                return RedirectToAction("Index");
            }

            // Handle case where user is already enrolled
            ModelState.AddModelError("", "You are already enrolled in this course.");
            return RedirectToAction("MyCourses");
        }

        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var enrolledCourses = await _enrollmentService.GetUserEnrolledCoursesAsync(userId);
            return View(enrolledCourses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new CourseViewModel
            {
                Title = course.Title,
                Description = course.Description,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                InstructorId = course.InstructorId,
                Instructor = course.Instructor
            };

            return View(viewModel);
        }

    }
}
