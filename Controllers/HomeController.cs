using Microsoft.AspNetCore.Mvc;
using MvcE_Learning.Models;
using System.Diagnostics;

namespace MvcE_Learning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult LoginPageOption()
        {
            return View();
        }

        public IActionResult RedirectInstructor()
        {
            return Redirect("/Instructors/Login");
        }

        public IActionResult RedirectStudent()
        {
            return Redirect("/Students/Login");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
