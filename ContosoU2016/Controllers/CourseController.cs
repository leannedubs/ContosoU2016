using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU2016.Data;
using ContosoU2016.Models;
using Microsoft.AspNetCore.Authorization;

namespace ContosoU2016.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly SchoolContext _context;

        public CourseController(SchoolContext context)
        {
            _context = context;
        }

        // lwilliston:  Reading related data
        // build a custom methof to return a sorted list of departments for our dropdown filter.

        private IQueryable<Course> GetCourses(int? SelectedDepartment)
        {
            // get all departments sorted by name 
            var departments = _context.Departments.OrderBy(d => d.Name).ToList();

            // add ViewData for use within View
            ViewData["SelectedDepartment"] = new SelectList(departments, "DepartmentID", "Name", SelectedDepartment);

            // retrieve the value of incoming parameter (SelectedDepartment)
            int departmentId = SelectedDepartment.GetValueOrDefault();

            // get courses belonging to that department
            IQueryable<Course> courses = _context.Courses
                // where() or where(DepartmentID == 1)
                .Where(c => !SelectedDepartment.HasValue || c.DepartmentID == departmentId)
                .OrderBy(d => d.CourseID)
                .Include(d => d.Departmemt);

            return courses;
        }

        //// GET: Course
        //public async Task<IActionResult> Index()
        //{
        //    var schoolContext = _context.Courses.Include(c => c.Departmemt);
        //    return View(await schoolContext.ToListAsync());
        //}


        public async Task<IActionResult> Index(int? SelectedDepartment)
        {
            //  The selectedDepartment refers to a Select box (dropdown) within our view. 
            IQueryable < Course > courses = GetCourses(SelectedDepartment);
            return View(await courses.ToListAsync());
        }

        // GET: Course/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Departmemt)
                .SingleOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name");
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.SingleOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Title,Credits,DepartmentID")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "Name", course.DepartmentID);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Departmemt)
                .SingleOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(m => m.CourseID == id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        // lwilliston
        [AllowAnonymous]
        public async Task<IActionResult> Listing(int? SelectedDepartment)
        {
            var courses = GetCourses(SelectedDepartment);
            return View(await courses.ToListAsync());
        }
        

            private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
