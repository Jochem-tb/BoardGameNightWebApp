using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using System.Security.Principal;

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
        
        public async Task<IActionResult> GameNightDetails(int gameNightId)
        {
            var gameNightDetails = await _gameNightService.GetByIdAsync(gameNightId);
            return View(gameNightDetails);
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
                    return RedirectToAction("GameNightDetails", gameNightId);
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

                //TODO: Fix the redirection to login page
                //return RedirectToAction("Login", $"Identity/Account");

                //Temporary fix
                return RedirectToAction("List");

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
