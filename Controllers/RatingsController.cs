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
    public class RatingsController : Controller
    {
        private readonly RatingDbContext _context;
        private readonly CourseDbContext _courseContext;

        public RatingsController(RatingDbContext context , CourseDbContext course)
        {
            _context = context;
            _courseContext = course;
        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rating.ToListAsync());
        }

        // GET: Ratings/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .FirstOrDefaultAsync(m => m.RatingId == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // GET: Ratings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id , [Bind("RatingId, rating")] Rating ratingInput)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("Rating is: " + ratingInput.rating);
                ratingInput.StudentId = Int32.Parse(Request.Cookies["Id"]);
                ratingInput.CourseId = id;
                _context.Add(ratingInput);

                await _context.SaveChangesAsync();

                var courses = from c in _courseContext.Course
                              select c;
                courses = courses.Where(c => c.CourseId == id);

                var course = courses.First();

                var ratings = from r in _context.Rating
                              select r;

                ratings = ratings.Where(r => r.CourseId == id);

                float[] allCourseRatings = new float[ratings.Count()];
                int counter = 0;
                foreach(Rating r in ratings)
                {
                    allCourseRatings[counter++] = r.rating;
                }
                float sum = 0;
                for(int i = 0; i < allCourseRatings.Length; i++)
                {
                    sum += allCourseRatings[i];
                }

                float average = sum / allCourseRatings.Length;

                course.Rating = average;

                await _courseContext.SaveChangesAsync();

                return Redirect("/Students/Home");
            }
            return Redirect("/Students/Home");
        }

        // GET: Ratings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RatingId , rating")] Rating ratingInput)
        {
            if (id != ratingInput.RatingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ratingsArr = from r in _context.Rating
                                     select r;
                    ratingsArr = ratingsArr.Where(r => r.RatingId == id);
                    var ratingSingular = ratingsArr.FirstOrDefault();

                    ratingSingular.rating = ratingInput.rating;

                    await _context.SaveChangesAsync();

                    var courses = from c in _courseContext.Course
                                  select c;



                    
                    Console.WriteLine("Ratings arr size is: " + ratingsArr.Count());
                    Console.WriteLine("Singular Rating Details: " + ratingSingular.RatingId + " " + ratingSingular.CourseId + " " + ratingSingular.rating);
                    var ratingCourseId = ratingSingular.CourseId;

                    Console.WriteLine("Course ID Derived is: " + ratingCourseId);

                    courses = courses.Where(c => c.CourseId == ratingCourseId);

                    var course = courses.First();

                    var ratings = from r in _context.Rating
                                  select r;

                    ratings = ratings.Where(r => r.CourseId == ratingCourseId);

                    float[] allCourseRatings = new float[ratings.Count()];
                    int counter = 0;
                    foreach (Rating r in ratings)
                    {
                        allCourseRatings[counter++] = r.rating;
                    }
                    float sum = 0;
                    for (int i = 0; i < allCourseRatings.Length; i++)
                    {
                        sum += allCourseRatings[i];
                    }

                    float average = sum / allCourseRatings.Length;

                    course.Rating = average;

                    await _courseContext.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(ratingInput.RatingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/Students/Home");
            }
            return Redirect("/Students/Home");
        }

        // GET: Ratings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .FirstOrDefaultAsync(m => m.RatingId == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = await _context.Rating.FindAsync(id);
            if (rating != null)
            {
                _context.Rating.Remove(rating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(e => e.RatingId == id);
        }
    }
}
