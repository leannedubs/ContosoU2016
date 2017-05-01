using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU2016.Data;
using ContosoU2016.Models;
using ContosoU2016.Helpers;

namespace ContosoU2016.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolContext _context;

        public StudentController(SchoolContext context)
        {
            _context = context;    
        }

        // GET: Student
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            // sortOrder: for sorting
            // searchString: for searching
            // currentFilter: to keep current search
            // page: for paging - optional argument (?)

            ViewData["CurrentSort"] = sortOrder;

            //return View(await _context.Students.ToListAsync());
            // LWilliston - add paging, sorting, and filtering functionality. 
            var students = from s in _context.Students
                           select s; // SELECT FirstName, LastName, Email, EnrollmentDate FROM Students

            // Part One:  For Sorting

            // Default sort = LastName sort
            ViewData["LNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lname_desc" : "";

            // Other Sort Orders. // Toggle between ascending and descending. 
            ViewData["FNameSortParam"] = sortOrder == "fname" ? "fname_desc" : "fname";
            ViewData["EmailSortParam"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";


            // Part Two:  Filtering
            if(searchString == null)
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1; // start on first page. 

                //  If the search string is changed during the paging, the page has to be reset to 1 
                //  because the new filter can result in different data to display. 
          
            }
            
            ViewData["CurrentFilter"] = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                // User entered a search criteria -> search by last name or first name
                students = students.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
            }

            // Apply the sorting 
            switch (sortOrder)
            {
                case "lname_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "fname":
                    students = students.OrderBy(s => s.FirstName);
                    break;
                case "fname_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                case "email":
                    students = students.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    students = students.OrderByDescending(s => s.Email);
                    break;
                case "date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            } // end of swtich case

            // lwilliston: changed to use paginated list
            //return View(await students.ToListAsync());
            int pageSize = 5;
            return View(await PaginatedList<Student>.CreateAsync(students, page ?? 1, pageSize));

            //  double question mark is the null coalescing operator
            //  pass the value of page, unless page is null, in which case it is assigned the value of 1
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentDate,ID,LastName,FirstName,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentDate,ID,LastName,FirstName,Email")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.SingleOrDefaultAsync(m => m.ID == id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
