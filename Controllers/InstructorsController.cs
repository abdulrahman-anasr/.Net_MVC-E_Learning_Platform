using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Data;
using MvcE_Learning.Models;
using System.Net;

namespace MvcE_Learning.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly InstructorsDbContext _context;
        private readonly CourseDbContext _courseContext;
        private readonly TeachDbContext _teachContext;

        public InstructorsController(InstructorsDbContext context, CourseDbContext courseContext, TeachDbContext teachContext)
        {
            _context = context;
            _courseContext = courseContext;
            _teachContext = teachContext;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instructors.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }


        // GET: Instructors/Home
        public async Task<IActionResult> Home()
        {
            var courses = from c in _courseContext.Course
                          select c;
            var teaches = from t in _teachContext.Teach
                          select t;
            teaches = teaches.Where(t => t.InstructorId == Int32.Parse(Request.Cookies["Id"]));
            Console.WriteLine("Hello Hello");
            int[] courseIdList = new int[teaches.Count()];
            int counter = 0;
            foreach(Teach t in teaches)
            {
                courseIdList[counter] = t.CourseId;
                counter++;
            }

            courses = courses.Where(c => courseIdList.Contains(c.CourseId));

            Console.WriteLine("Courses is: " + courses.GetType().Name);

            var coursesEnumerable = courses.AsEnumerable();
            return View(coursesEnumerable);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorId,Name,Email,Password,FieldOfWork,YearsOfExperience")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
                
            }
            return View(instructor);
        }

        // POST: Instructors/Create
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] Instructor instructor)
        {
            if (string.IsNullOrEmpty(instructor.Email) && string.IsNullOrEmpty(instructor.Password))
            {
                return BadRequest();
            }
            var instructorSearch = from i in _context.Instructors
                                   select i;
            instructorSearch = instructorSearch.Where(s =>  s.Email == instructor.Email);
            instructorSearch = instructorSearch.Where(s =>s.Password == instructor.Password);

            Instructor returnInstructor = instructorSearch.FirstOrDefault();

            if(returnInstructor != null)
            {
                Console.WriteLine(returnInstructor.Email);
                HttpContext.Response.Cookies.Append("Role", "Instructor");
                HttpContext.Response.Cookies.Append("Name", returnInstructor.Name);
                HttpContext.Response.Cookies.Append("Id", returnInstructor.InstructorId.ToString());
                return Redirect("/Instructors/Home");

            }
            else
            {
                InstructorLoginSuccess instructorLoginSuccess = new InstructorLoginSuccess();
                instructorLoginSuccess.success = false;
                return View(instructorLoginSuccess);

            }

            
        }

        // POST: Instructors/LogOut
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("Role");
            HttpContext.Response.Cookies.Delete("Registered");
            HttpContext.Response.Cookies.Delete("Name");
            return Redirect("/");
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorId,Name,Email,Password,FieldOfWork,YearsOfExperience")] Instructor instructor)
        {
            if (id != instructor.InstructorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.InstructorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorId == id);
        }
    }
}
