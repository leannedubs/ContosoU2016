using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU2016.Data;
using ContosoU2016.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ContosoU2016.Controllers
{
    //[Authorize]
    [Authorize(Roles = "student")] // must be logging in as student role. 
    public class StudentEnrollmentController : Controller
    {
        private readonly SchoolContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // lwilliston: need identity users 

        public StudentEnrollmentController(SchoolContext context, UserManager<ApplicationUser> userManager)
            // lwilliston:  Instantiate the userManager
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: StudentEnrollment
        public async Task<IActionResult> Index()
        {
            // lwilliston: retrieve currently logged in student
            var user = await GetCurrentUserAsync();
            if(user == null)
            {
                return NotFound();  // TO DO: Return Error View 
                // return View("Error");
            };

            // locate logged in user (student) within the student entity
            var student = await _context.Students
                                .Include(s => s.Enrollments)
                                .ThenInclude(e => e.Course)
                                .AsNoTracking()
                                .SingleOrDefaultAsync(m=>m.Email == user.Email);
            //  We may need to associate with a user.Id

            // 1. Courses Enrolled :  (student is enrolled in these)
            var studentEnrollments = _context.Enrollments
                                     .Include(c => c.Course)
                                     .Include(c => c.Student)
                                     .Where(c => c.StudentID == student.ID)
                                     .AsNoTracking();

            ViewData["StudentName"] = student.FullName;

            // 2. Courses Available: (student is NOT enrolled in these)

            string query = "SELECT CourseID, Credits, Title, DepartmentID FROM Course WHERE CourseID NOT IN (SELECT DISTINCT CourseID FROM Enrollment WHERE StudentID = {0})";

            // building raw SQL Query using LINQ (Language INtergrated Query)
            var courses = _context.Courses.FromSql(query, student.ID).AsNoTracking();

            ViewBag.Courses = courses.ToList(); // Could also write as  ->  ViewData["Courses"] = courses.ToList();

            return View(await studentEnrollments.ToListAsync());
        }


        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        // GET : Enroll
        public async Task<IActionResult> Enroll(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = await GetCurrentUserAsync();
            if(user == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Email == user.Email);

            //Return student ID to view using ViewData
            ViewData["StudentID"] = student.ID; // for hidden field in form (so we know who they are)

            // retrieve this students current enrollment (to compare against the course they want to enroll in) 
            var studentEnrollments = new HashSet<int>(_context.Enrollments.Include(e => e.Course)
                                                              //.Include(e => e.Student)
                                                              .Where(e => e.StudentID == student.ID)
                                                              .Select(e => e.CourseID));
            // conversion from int? to int is not possibe (need int)
            int currentCourseID;
            if (id.HasValue)
            {
                currentCourseID = (int)id;
            }
            else
            {
                currentCourseID = 0;
            }
            // end conversion fix

            // student tries to enroll in a course they are already in. 
            if (studentEnrollments.Contains(currentCourseID))
            {
                // student is trying to enroll in a course they are already enrolled in.  Send a model state error back to view. 
                ModelState.AddModelError("AlreadyEnrolled", "You are already enrolled in this course.");
            }

            // student tries to enroll in a course that does not exist. 
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseID == id);
            if(course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Enroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll([Bind("CourseID, StudentID")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
                
            }
            return View(enrollment);
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentID == id);
        }
    }
}
