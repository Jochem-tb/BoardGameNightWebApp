using BGN.Services.Abstractions;
using BGN.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IServiceManager serviceManager)
        {
            _gameService = serviceManager.GameService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            var games = await _gameService.GetAllAsync();
            return View(new GameListModel() { DisplayGames = games});
        }

        public IActionResult Details()
        {
            //TODO: Implement GameDetails Method
            return View();
        }

        public async Task<IActionResult> CategoryId(int catId)
        {
            var games = await _gameService.GetAllGameByCategoryIdAsync(catId);
            return View("List", new GameListModel() { DisplayGames = games});
        }

        public async Task<IActionResult> GenreId(int genId)
        {
            var games = await _gameService.GetAllGameByGenreIdAsync(genId);
            return View("List", new GameListModel() { DisplayGames = games});
        }

        [HttpGet]
        public async Task<IActionResult> Filter(GameListModel gameListModel)
        {
            var games = await _gameService.GetAllAsync(gameListModel);
            return View("List", new GameListModel() { DisplayGames = games});
        }
    }
}
