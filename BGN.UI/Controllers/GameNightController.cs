using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BGN.UI.Controllers
{
    [AllowAnonymous]
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

        [Authorize]
        public async Task<IActionResult> Join(int gameNightId)
        {
            //Get the current user's ID
            var identityUserKey = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(identityUserKey != null)
            {
                //Call the service to join the game night
                var result = await _gameNightService.JoinGameNightAsync(gameNightId, identityUserKey);

                if (result)
                {
                    return RedirectToAction("GameNightDetails");
                }
                else
                {
                    //Handle case where user is already attending or game night is full
                    TempData["JoinGameNightError"] = "You are already attending this game night or it is full.";
                    return RedirectToAction("List");
                }
            }

            else
            {
                //Handle case where user is not authenticated
                TempData["JoinGameNightError"] = "You must be logged in to join a game night.";
                return RedirectToAction("Login", "Identity/Account");
            }
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
