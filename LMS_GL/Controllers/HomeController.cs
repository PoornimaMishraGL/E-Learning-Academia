using LMS_GL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LMS_GL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public LMSContext context;

        public HomeController(ILogger<HomeController> logger, LMSContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.courses.ToList());
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Says()
        {
            return View();
        }
        public IActionResult OurTeam()
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