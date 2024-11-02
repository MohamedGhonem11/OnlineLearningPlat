using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineLearning.Entity.Entities;
using OnlineLearning.Service.Interfaces;
using OnlineLearning.Service.ViewModels;

using System.Linq;
using System.Threading.Tasks;

namespace OnlineLearning.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly IProgressTrackingService _progressTrackingService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAssignmentService _assignmentService;




        public CoursesController(ICourseService courseService, IUserService userService, IProgressTrackingService progressTrackingService, UserManager<ApplicationUser> userManager, IAssignmentService assignmentService)
        {
            _courseService = courseService;
            _userService = userService;
            _progressTrackingService = progressTrackingService;
            _userManager = userManager;
            _assignmentService = assignmentService;
        }

        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> InstructorCourses()
        {

            var instructorId = _userManager.GetUserId(User);

            // Fetch courses taught by the instructor with the count of enrolled students
            var courses = await _courseService.GetCoursesWithEnrollmentCountAsync(instructorId);

            return View(courses); // Pass the data to the view
        }


        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> CourseProgress(int courseId)
        {

            var progressData = await _progressTrackingService.GetCourseProgressAsync(courseId); // Fetch student progress data for the course

            return View(progressData); // Pass data to the view
        }


        // Action for instructors to view progress tracking for their courses
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> ProgressTracking(int courseId)
        {
            var progressData = await _progressTrackingService.GetCourseProgressAsync(courseId); // Fetch progress data
            return View(progressData); // Pass data to the view
        }

        // Action for instructors to update student progress
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateProgress(int progressId, bool isCompleted, int courseId)
        {
            if (progressId == 0 || courseId == 0)
            {
                // Log the issue
                Console.WriteLine("Invalid progressId or courseId");
                TempData["ErrorMessage"] = "Invalid progressId or courseId. Please try again.";
                return RedirectToAction("CourseProgress", new { courseId });
            }

            // Update the progress status for the specific progress entry
            await _progressTrackingService.UpdateProgressAsync(progressId, isCompleted);

            // Redirect back to the course progress view after updating the progress
            return RedirectToAction("CourseProgress", new { courseId = courseId });
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateAssignmentProgress(int progressId, bool isCompleted, int courseId, string userId)
        {
            if (progressId == 0 || courseId == 0 || string.IsNullOrEmpty(userId))
            {
                Console.WriteLine($"Invalid parameters - progressId: {progressId}, courseId: {courseId}, userId: {userId}");
                TempData["ErrorMessage"] = "Invalid progressId, courseId, or userId. Please try again.";
                return RedirectToAction("ViewStudentAssignments", new { courseId, userId });
            }

            // Update the progress status for the specific assignment entry
            await _progressTrackingService.UpdateProgressAsync(progressId, isCompleted);

            // Redirect back to the student assignments view after updating the progress
            return RedirectToAction("ViewStudentAssignments", new { courseId, userId });
        }

        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> EditAssignment(int assignmentId)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            var model = new AssignmentViewModel
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                CourseId = assignment.CourseId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> EditAssignment(int id, AssignmentViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
                if (assignment == null)
                {
                    return NotFound();
                }

                assignment.Title = model.Title;
                assignment.Description = model.Description;

                await _assignmentService.UpdateAssignmentAsync(assignment);

                return RedirectToAction("ManageAssignments", new { courseId = assignment.CourseId });
            }

            return View(model);
        }


        // GET: /Courses/DeleteAssignment/{assignmentId}
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteAssignment(int assignmentId)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment); // This should return a view to confirm deletion
        }

        [HttpPost, ActionName("DeleteAssignmentConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteAssignmentConfirmed(int assignmentId)
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId);
            if (assignment == null)
            {
                return NotFound();
            }

            await _assignmentService.DeleteAssignmentAsync(assignmentId);

            return RedirectToAction("ManageAssignments", new { courseId = assignment.CourseId });
        }



        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> ManageAssignments(int courseId)
        {
            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var assignments = await _assignmentService.GetAssignmentsForCourse(courseId);
            ViewBag.CourseTitle = course.Title; // To display the course title in the view
            ViewBag.CourseId = courseId; // To pass the course ID for adding assignments

            return View(assignments);
        }



        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddAssignment(int courseId)
        {
            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var model = new AssignmentViewModel
            {
                CourseId = courseId  // Assign course ID to the view model
            };

            return View(model);
        }

        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> ManageCourses()
        {
            var instructorId = _userManager.GetUserId(User);

            // Fetch courses taught by the instructor
            var courses = await _courseService.GetCoursesWithEnrollmentCountAsync(instructorId);

            return View(courses); // Pass the courses to the view
        }


        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAssignment(AssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var assignment = new Assignment
                {
                    Title = model.Title,
                    Description = model.Description,
                    CourseId = model.CourseId
                };

                await _courseService.AddAssignmentAsync(assignment);  // Service method to save assignment

                // Initialize progress tracking for each student enrolled in the course
                await _progressTrackingService.InitializeProgressTracking(model.CourseId);

                return RedirectToAction("ManageCourses");
            }

            return View(model);
        }





        // GET: /Courses/Index
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        // GET: /Courses/Details/{id}
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

        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> ViewStudentAssignments(int courseId, string userId)
        {
            if (courseId == 0 || string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid courseId or userId");
            }

            // Call service to get assignments for the student
            var assignments = await _progressTrackingService.GetAssignmentsForStudentAsync(courseId, userId);

            if (assignments == null || assignments.Count == 0)
            {
                return NotFound("No assignments found for the student in this course.");
            }

            return View(assignments);
        }




        // GET: /Courses/Create
        public async Task<IActionResult> Create()
        {
            await PopulateInstructorsDropDownList();
            return View();
        }

        // POST: /Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {

            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                InstructorId = model.InstructorId,
            };

            await _courseService.AddCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Courses/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var model = new CourseViewModel
            {
                Title = course.Title,
                Description = course.Description,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                InstructorId = course.InstructorId
            };

            await PopulateInstructorsDropDownList();
            return View(model);
        }

        // POST: /Courses/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel model)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            course.Title = model.Title;
            course.Description = model.Description;
            course.StartDate = model.StartDate;
            course.EndDate = model.EndDate;
            course.InstructorId = model.InstructorId;

            await _courseService.UpdateCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Courses/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course); // Pass the course to the view for confirmation.
        }

        // POST: /Courses/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            await _courseService.DeleteCourseAsync(id);
            TempData["SuccessMessage"] = "Course deleted successfully."; // Optional: Success message.

            return RedirectToAction(nameof(Index));
        }

        // Helper method to populate the dropdown for instructors
        private async Task PopulateInstructorsDropDownList()
        {
            var instructors = await _userService.GetAllInstructorsAsync();
            ViewBag.Instructors = new SelectList(instructors, "Id", "FullName");
        }




    }
}