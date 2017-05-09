using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU2016.Data;
using ContosoU2016.Models;
using ContosoU2016.Models.SchoolViewModels;

namespace ContosoU2016.Controllers
{
    public class InstructorController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index(int? id, int? courseID) // add param for selected instructor(id)
                                                                       // add param for selected Course(id)
        {
            // Original Scaffolded code 
            //return View(await _context.Instructors.ToListAsync());

            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment) // include Offices assigned to Instructor
                                                  // ---- Enrollment ---- //
                .Include(i => i.Courses) // within courses property load the enrollments
                    .ThenInclude(i => i.Course) // have to get the course entity out of the Courses join entity.
                        .ThenInclude(i => i.Departmemt)
                .OrderBy(i => i.LastName) //sort by instructor last name asc
                .ToListAsync();

            // ---------- Instructor Selected ---------- // 
            if (id != null)
            {
                Instructor instructor = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
                viewModel.Courses = instructor.Courses.Select(s => s.Course);

                // get instructor name for display in view
                ViewData["InstructorName"] = instructor.FullName;

                // return the ID back to the view to hightlight the selected row. 
                @ViewData["InstructorID"] = id.Value; // or @ViewData["InstructorData"] = instructor.ID

            }
            // ---------- End Instructor Selected ---------- //

            // ---------- Course Selected ---------- //
            if (courseID != null)
            {
                _context.Enrollments.Include(i => i.Student).Where(c => c.CourseID == courseID.Value).Load();
                viewModel.Enrollments = viewModel.Courses.Where(x => x.CourseID == courseID).Single().Enrollments;

                ViewData["CourseID"] = courseID.Value;
            }
            // ---------- End Course Selected ---------- //


            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i=>i.OfficeAssignment) // lwilliston : included office 
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.Courses = new List<CourseAssignment>();
            // populate the assignedCourseData View Model
            PopulateAssignedCourseData(instructor);
            return View();
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            // get all courses
            var allCourses = _context.Courses;

            // create hashset of instructor corses.
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));

            // create and populate the AssignedCourseData ViewModel
            var viewModel = new List<AssignCourseData>();

            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }

            // save the viewmodel with the ViewData object for use within VIew
            ViewData["Courses"] = viewModel;

        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HireDate,LastName,FirstName,Email,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            // lwilliston: added string[] selectedCourses method argument for course assignments 
            if (selectedCourses != null)
            {
                instructor.Courses = new List<CourseAssignment>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment
                    {
                        InstructorID = instructor.ID,
                        CourseID = int.Parse(course)
                    };
                    instructor.Courses.Add(courseToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.Include(i => i.OfficeAssignment).Include(i => i.Courses).SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            // populate the assignedCourseData View Model
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        {
            // lwilliston: take care of overposting and added selectedCourses string[] argument
            if (id == null)
            {
                return NotFound();
            }

            // find the instructor to update (because of overposting check) 
            var instructorToUpdate = await _context.Instructors.Include(i => i.OfficeAssignment)
                                                               .Include(i => i.Courses).ThenInclude(i => i.Course)
                                                               .SingleOrDefaultAsync(i => i.ID == id);
            if (await TryUpdateModelAsync<Instructor>(
                instructorToUpdate, "", i => i.FirstName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment
                ))
            {
                // Check for empty string on Office Location
                if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }

                // Update Courses
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                if (ModelState.IsValid) { 
                // Save changes  (try...catch)
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /*ex*/)
                {

                    //We could log the error using the ex argument. 
                    // lets simply return a model state error back to the view.
                    ModelState.AddModelError("", "Unable to save changes.");
                }
                return RedirectToAction("Index");
                }
            }
            return View(instructorToUpdate);
        }
        

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if(selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<CourseAssignment>();
                return;
            }
            
            var selectedCourseHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>(instructorToUpdate.Courses.Select(c => c.Course.CourseID));

            // loop through all courses in the database and check each course against the ones currently assigned to the instructor
            // vs the ones that were selected in the view.
            foreach(var course in _context.Courses)
            {
                if (selectedCourseHS.Contains(course.CourseID.ToString())) // condition 1.  Check if something is selected NOW that wasn't before.
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Add(new CourseAssignment
                        {
                            InstructorID = instructorToUpdate.ID,
                            CourseID = course.CourseID
                        });
                    }
                }
                else // condition 2. Check if something WAS selected before, but isn't now.
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        CourseAssignment courseToRemove = instructorToUpdate.Courses.SingleOrDefault(i => i.CourseID == course.CourseID);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.SingleOrDefaultAsync(m => m.ID == id);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}
