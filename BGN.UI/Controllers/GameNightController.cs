using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.UI.Models;
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
            return View(new GameNightListModel() { DisplayGameNights = gameNightList });
        }

        [HttpGet]
        public async Task<IActionResult> Filter(GameNightListModel gameNightListModel)
        {

            // If the model state is valid, fetch games based on the filter criteria
            //GetAllAsync is overloaded, so we can pass the GameListModel to it
            var filteredGameNights = await _gameNightService.GetAllAsync(gameNightListModel);
            gameNightListModel.DisplayGameNights = filteredGameNights;

            // Return the filtered games to the view
            return View("List", gameNightListModel);
        }
    }
}
