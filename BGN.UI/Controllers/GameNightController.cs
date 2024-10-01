using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.Controllers
{
    public class GameNightController : Controller
    {
        private readonly  IGameNightService _gameNightService;
        public GameNightController(IServiceManager serviceManager)
        {
            _gameNightService = serviceManager.GameNightService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var gameNightList = await _gameNightService.GetAllAsync();
            return View(gameNightList);
        }
    }
}
