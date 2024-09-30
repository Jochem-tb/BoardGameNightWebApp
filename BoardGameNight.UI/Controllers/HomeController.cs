using BoardGameNight.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BoardGameNight.Domain;


namespace BoardGameNight.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "fiets",
                HouseNr = "123",
                PostalCode = "1234AD"
            };
            ViewBag.Person = person;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
