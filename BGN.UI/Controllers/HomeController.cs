using BGN.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using BGN.Services.Abstractions;

namespace BGN.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IServiceManager serviceManager) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IServiceManager _serviceManager = serviceManager;

        public IActionResult Index()
        {
           
            return View();
        }

        public async Task<IActionResult> PrivacyAsync()
        {
            var person1 = await _serviceManager.PersonService.GetByIdAsync(3);
            ViewBag.Person = person1;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
