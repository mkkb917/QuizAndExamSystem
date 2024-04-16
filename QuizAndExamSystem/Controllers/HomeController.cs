using ExamSystem.Filters;
using ExamSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }

        public IActionResult Notes()
        {
            _logger.LogInformation("Notes page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }
        public IActionResult Plans()
        {
            _logger.LogInformation("Notes page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }
        public IActionResult Notifications()
        {
            _logger.LogInformation("Notifications page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Privacy page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }
        public IActionResult Terms()
        {
            _logger.LogInformation("Terms and Condtions page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }

        public IActionResult About()
        {
            _logger.LogInformation("About page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }

        public IActionResult FAQ()
        {
            _logger.LogInformation("FAQ page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }

        public IActionResult Contact()
        {
            _logger.LogInformation("Contact page of Home Controller is accessed by {0}", User.Identity.Name);
            return View();
        }
        public IActionResult Career()
        {
            _logger.LogInformation("Career page of Home Controller is accessed by {0}", User.Identity.Name);
            return View("NotFound");
        }

        public IActionResult Investor()
        {
            _logger.LogInformation("Investor page of Home Controller is accessed by {0}", User.Identity.Name);
            return View("NotFound");
        }
        public IActionResult Team()
        {
            _logger.LogInformation("team page of Home Controller is accessed by {0}", User.Identity.Name);
            return View("NotFound");
        }
    }
}