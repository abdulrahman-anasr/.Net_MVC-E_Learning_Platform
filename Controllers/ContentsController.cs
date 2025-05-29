using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.EntityFrameworkCore;
using MvcE_Learning.Data;
using MvcE_Learning.Migrations;
using MvcE_Learning.Models;

namespace MvcE_Learning.Controllers
{
    public class ContentsController : Controller
    {
        private readonly ContentDbContext _context;
        private readonly CourseDbContext _CoursesContext;

        public ContentsController(ContentDbContext context, CourseDbContext coursesContext )
        {
            _CoursesContext = coursesContext;
            _context = context;
        }

        // GET: Contents
        public async Task<IActionResult> Index()
        {

            return View(await _context.Content.ToListAsync());
        }

        // GET: Contents/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .FirstOrDefaultAsync(m => m.ContentId == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Contents/Create
        public IActionResult Create(int courseId)
        {
            ViewData["CourseId"] = courseId;
            return View();
        }

        // POST: Contents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("Title,DateAdded,Type,CourseId,PdfUpload")] Content content)
        {
            content.CourseId = (int)id;



            if (ModelState.IsValid)
            {
                if (content.PdfUpload != null && content.PdfUpload.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await content.PdfUpload.CopyToAsync(memoryStream);
                        content.pdfFile = memoryStream.ToArray();
                        Console.WriteLine("Im here");
                        Console.WriteLine("HERE HERE HERE HERE :" + memoryStream.ToArray()); // Vodoo Shit
                    }
                }
                _context.Add(content);
                await _context.SaveChangesAsync();
                return Redirect("/Courses/Details/" + id);
            }
            else
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine($"Model state invalid: {errors}");
            }
            Console.WriteLine($"Title: {content.Title}, DateAdded: {content.DateAdded}, Type: {content.Type}, CourseId: {content.CourseId}, pdfFile: {content.pdfFile}");
            return View(content);
        }

        [HttpGet("Download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var courses = from c in _CoursesContext.Course
                          select c;
            courses = courses.Where(c => c.CourseId == id);
            var course = courses.FirstOrDefault();


            var content = from c in _context.Content
                          where c.CourseId == id
                          select c;
            var contents = content.FirstOrDefault();


            if (content == null || contents.pdfFile == null)
            {
                return NotFound(); 
            }

            // Set the response content type for PDF
            Response.ContentType = "application/pdf";
            Response.Headers.Add("Content-Disposition", "attachment; filename=content.pdf"); 

            return File(contents.pdfFile, "application/pdf");
        }

        // GET: Contents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContentId,Title,DateAdded,Type,CourseId")] Content content)
        {
            if (id != content.ContentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.ContentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { courseId = content.CourseId });

            }
            return View(content);
        }

        // GET: Contents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Content
                .FirstOrDefaultAsync(m => m.ContentId == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _context.Content.FindAsync(id);
            if (content != null)
            {
                _context.Content.Remove(content);
            }

            await _context.SaveChangesAsync();
            return Redirect("/Instructors/Home");

        }

        private bool ContentExists(int id)
        {
            return _context.Content.Any(e => e.ContentId == id);
        }
    }
}
