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
    public class CoursesController : Controller
    {
        private readonly CourseDbContext _context;
        private readonly InstructorsDbContext _instructorContext;
        private readonly TeachDbContext _teachContext;
        private readonly ContentDbContext _contentContext;

        public CoursesController(CourseDbContext context, InstructorsDbContext instructorContext, TeachDbContext teachContext, ContentDbContext contentContext)
        {
            _context = context;
            _instructorContext = instructorContext;
            _teachContext = teachContext;
            _contentContext = contentContext;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string courseFields, string searchString)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'MvcE_Learning.Course'  is null.");
            }

            
            IQueryable<string> fieldsQuery = from m in _context.Course
                                            orderby m.Fields
                                            select m.Fields;
            var courses = from c in _context.Course
                         select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(courseFields))
            {
                courses = courses.Where(x => x.Fields == courseFields);
            }

            var courseFieldVM = new CourseViewModel
            {
                Fields = new SelectList(await fieldsQuery.Distinct().ToListAsync()),
                Course = await courses.ToListAsync()
            };

            return View(courseFieldVM);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courses = from c in _context.Course
                          select c;

            courses = courses.Where(c => c.CourseId == id);

            var course =  courses.FirstOrDefault();

            var content = from c in _contentContext.Content
                          where c.CourseId == id
                          select c;

            ICollection<Content> contents = content.ToList();

            CourseContentViewModel courseContentViewModel = new CourseContentViewModel();

            courseContentViewModel.Course = course;
            courseContentViewModel.Contents = contents;


            if (course == null)
            {
                return NotFound();
            }

            return View(courseContentViewModel);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("courseId,Name,Description,Duration,Difficulty,Fields,Rating")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                var courses = from c in _context.Course
                              select c;

                courses = courses.Where(i => i.Name == course.Name);
                courses = courses.Where(i => i.Description == course.Description);
                var courseId = courses.FirstOrDefault().CourseId;
                Teach teach = new Teach();
                teach.CourseId = courseId;
                teach.InstructorId = Int32.Parse(Request.Cookies["Id"]);
                _teachContext.Add(teach);

                await _teachContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("courseId,Name,Description,Duration,Difficulty,Fields,Rating")] Course course)
        {
            if (id != course.CourseId)
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
                    if (!CourseExists(course.CourseId))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
