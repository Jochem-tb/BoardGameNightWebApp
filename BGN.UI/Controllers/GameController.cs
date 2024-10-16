using BGN.Domain.Entities;
using BGN.Services;
using BGN.Services.Abstractions;
using BGN.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IGameNightService _gameNightService;
        private readonly IUserService _userService;

        public GameController(IServiceManager serviceManager, IUserService userService)
        {
            _gameService = serviceManager.GameService;
            _gameNightService = serviceManager.GameNightService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            //List is the base view
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            var games = await _gameService.GetAllAsync();
            var currentUser = await _userService.GetLoggedInUserAsync();
            return View(new GameListModel() { DisplayGames = games, CurrentUser = currentUser });



        }

        [Authorize]
        public async Task<IActionResult> GameDetails(int gameId)
        {
            //TODO: Implement GameDetails Method
            var gameDetails = await _gameService.GetByIdAsync(gameId);
            var currentUser = await _userService.GetLoggedInUserAsync();
            var gameNightWithGame = await _gameNightService.GetAllWithGameIdAsync(gameId);
            if(gameDetails == null)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View(new GameDetailsModel() { Game = gameDetails, CurrentUser = currentUser, GameNightsWithThisGame = gameNightWithGame });
            }
        }

        public async Task<IActionResult> CategoryId(int catId)
        {
            var games = await _gameService.GetAllGameByCategoryIdAsync(catId);
            var currentUser = await _userService.GetLoggedInUserAsync();

            //Return GameListModel with the games, so they will be displayed. 
            //Return also SelectedCategories, so the ViewComponents know to set the checkbox to checked
            return View("List", new GameListModel() { DisplayGames = games, SelectedCategories = new List<int>() { catId }, CurrentUser = currentUser });
        }

        public async Task<IActionResult> GenreId(int genId)
        {
            var games = await _gameService.GetAllGameByGenreIdAsync(genId);
            var currentUser = await _userService.GetLoggedInUserAsync();

            //Return GameListModel with the games, so they will be displayed. 
            //Return also SelectedGenres, so the ViewComponents know to set the checkbox to checked
            return View("List", new GameListModel() { DisplayGames = games, SelectedGenres = new List<int>() { genId }, CurrentUser = currentUser });
        }



        [HttpGet]
        public async Task<IActionResult> Filter(GameListModel gameListModel)
        {

            // Ensure selected genres and categories are also set
            gameListModel.SelectedGenres = gameListModel.SelectedGenres ?? new List<int>();
            gameListModel.SelectedCategories = gameListModel.SelectedCategories ?? new List<int>();

            // If the model state is valid, fetch games based on the filter criteria
            //GetAllAsync is overloaded, so we can pass the GameListModel to it
            var filteredGames = await _gameService.GetAllAsync(gameListModel);
            gameListModel.DisplayGames = filteredGames;

            // Return the filtered games to the view
            return View("List", gameListModel);
        }
    }
}
