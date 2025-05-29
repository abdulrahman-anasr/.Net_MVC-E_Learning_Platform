using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Data;
using MvcE_Learning.Models;

namespace MvcE_Learning.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentsDbContext _context;
        private readonly CourseDbContext _courseContext;
        private readonly EnrollDbContext _enrollContext;
        private readonly RatingDbContext _ratingContext;

        public StudentsController(StudentsDbContext context, CourseDbContext courseDbContext, EnrollDbContext enrollDbContext , RatingDbContext ratingDbContext)
        {
            _context = context;
            _courseContext = courseDbContext;
            _enrollContext = enrollDbContext;
            _ratingContext = ratingDbContext;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Home()
        {
            var userId = Int32.Parse(Request.Cookies["Id"]);
            var courses = from c in _courseContext.Course
                          select c;
            var enroll = from t in _enrollContext.Enroll
                          select t;

            var ratings = from t in _ratingContext.Rating
                          select t;
            ratings = ratings.Where(r => r.StudentId == userId);
            enroll = enroll.Where(t => t.StudentId == userId);
            Console.WriteLine("Hello Hello");
            int[] courseIdList = new int[enroll.Count()];
            int counter = 0;
            foreach (Enroll t in enroll)
            {
                courseIdList[counter] = t.CourseId;
                counter++;
            }

            courses = courses.Where(c => courseIdList.Contains(c.CourseId));
            ratings = ratings.Where(r => courseIdList.Contains(r.CourseId));
            int[] ratingsListCourseIds = new int[ratings.Count()];
            counter = 0;
            Console.WriteLine(ratings);
            foreach(Rating r in ratings)
            {
                ratingsListCourseIds[counter] = r.CourseId;
                counter++;
            }
            Console.WriteLine("Rating size is: " + ratings.Count());
 
            IEnumerable<CourseRatingViewModel> crvm = Enumerable.Empty<CourseRatingViewModel>();
            foreach(Course course in courses)
            {
                Console.WriteLine("Inside Loop");
                CourseRatingViewModel crvmEntity = new CourseRatingViewModel();
                crvmEntity.course = course;
                int courseId = crvmEntity.course.CourseId;
                if(ratingsListCourseIds.Contains(course.CourseId))
                {
                    crvmEntity.rated = true;
                    var Erating = ratings.Where(r => r.CourseId == courseId);
                    Erating = Erating.Where(r => r.StudentId == userId);
                    crvmEntity.rating = Erating.First().rating;
                    crvmEntity.ratingId = Erating.First().RatingId;
                }
                else
                {
                    crvmEntity.rated = false;
                }
                crvm = crvm.Append(crvmEntity);
                Console.WriteLine("Entity has details: " + crvmEntity.course.CourseId + " " + crvmEntity.rating);
            }
            Console.WriteLine("Courses size is: " + courses.Count());
            Console.WriteLine("CRVM Size is: " + crvm.Count());
            return View(crvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] Student student)
        {
            if (string.IsNullOrEmpty(student.Email) && string.IsNullOrEmpty(student.Password))
            {
                return BadRequest();
            }
            var studentSearch = from i in _context.Students
                                   select i;
            studentSearch = studentSearch.Where(s => s.Email == student.Email);
            studentSearch = studentSearch.Where(s => s.Password == student.Password);

            Student returnStudent = studentSearch.FirstOrDefault();

            if (returnStudent != null)
            {
                Console.WriteLine(returnStudent.Email);
                HttpContext.Response.Cookies.Append("Role", "Student");
                HttpContext.Response.Cookies.Append("Name", returnStudent.Name);
                HttpContext.Response.Cookies.Append("Id", returnStudent.StudentId.ToString());
                return Redirect("/Students/Home");
            }
            else
            {
                StudentLoginSuccess studentLoginSuccess = new StudentLoginSuccess();
                studentLoginSuccess.success = false;
                //studentLoginSuccess.student = new Student();
                //studentLoginSuccess.student.Email = student.Email;
                //studentLoginSuccess.student.Password = student.Password;
                return View(studentLoginSuccess);

            }
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentId = Int32.Parse(Request.Cookies["Id"]);
            Enroll enrolled = new Enroll();
            enrolled.StudentId = studentId;
            enrolled.CourseId = (int) id;
            _enrollContext.Add(enrolled);
            _enrollContext.SaveChanges();
            return Redirect("/Courses");
        }

        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("Role");
            HttpContext.Response.Cookies.Delete("Registered");
            HttpContext.Response.Cookies.Delete("Name");
            return Redirect("/");
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Name,Email,Password,Age,AreasOfInterest,EducationLevel")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                HttpContext.Response.Cookies.Append("Role", "Student");
                HttpContext.Response.Cookies.Append("Name", student.Name);
                HttpContext.Response.Cookies.Append("Id", student.StudentId.ToString());
                return RedirectToAction(nameof(Home));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,Name,Email,Password,Age,AreasOfInterest,EducationLevel")] Student student)
        {
            if (id != student.StudentId)
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
                    if (!StudentExists(student.StudentId))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
