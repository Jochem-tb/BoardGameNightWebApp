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
        private readonly IUserService _userService;

        public GameController(IServiceManager serviceManager, IUserService userService)
        {
            _gameService = serviceManager.GameService;
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

            if(currentUser != null)
            {
                //If user is logged in pass it throug 
                return View(new GameListModel() { DisplayGames = games, CurrentUser = currentUser});
            }
            else
            {
                //We dont specify currentUser because it is null
                return View(new GameListModel() { DisplayGames = games });
            }
            
        }

        [Authorize]
        public async Task<IActionResult> GameDetails(int gameId)
        {
            //TODO: Implement GameDetails Method
            var gameDetails = await _gameService.GetByIdAsync(gameId);
            var currentUser = await _userService.GetLoggedInUserAsync();

            return View(new GameDetailsModel() { Game = gameDetails, CurrentUser = currentUser });
        }

        public async Task<IActionResult> CategoryId(int catId)
        {
            var games = await _gameService.GetAllGameByCategoryIdAsync(catId);

            //Return GameListModel with the games, so they will be displayed. 
            //Return also SelectedCategories, so the ViewComponents know to set the checkbox to checked
            return View("List", new GameListModel() { DisplayGames = games, SelectedCategories = new List<int>() { catId } });
        }

        public async Task<IActionResult> GenreId(int genId)
        {
            var games = await _gameService.GetAllGameByGenreIdAsync(genId);

            //Return GameListModel with the games, so they will be displayed. 
            //Return also SelectedGenres, so the ViewComponents know to set the checkbox to checked
            return View("List", new GameListModel() { DisplayGames = games, SelectedGenres = new List<int>() { genId } });
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
