using BGN.Domain.Repositories;
using BGN.Services;
using BGN.Services.Abstractions;
using BGN.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using System.Security.Principal;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using BGN.Domain.Entities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BGN.UI.Controllers
{
    [Authorize]
    public class GameNightController : Controller
    {
        private readonly  IGameNightService _gameNightService;
        private readonly  IUserService _userService;
        private readonly  IGameService _gameService;
        private readonly  IMiscService _miscService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        private CrudGameNightModel? shareableCrudGameNightModel;
        public GameNightController(IServiceManager serviceManager, IUserService userService, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _gameNightService = serviceManager.GameNightService;
            _userService = userService;
            _gameService = serviceManager.GameService;
            _miscService = serviceManager.MiscService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        public async Task<IActionResult> List()
        {
            var gameNightList = await _gameNightService.GetAllAsync();
            var currentUser = await _userService.GetLoggedInUserAsync();
            return View(new GameNightListModel() { DisplayGameNights = gameNightList, CurrentUser = currentUser });
        }

        public async Task<IActionResult> GameNightDetails(int gameNightId)
        {
            var gameNightDetails = await _gameNightService.GetByIdAsync(gameNightId);
            var currentUser = await _userService.GetLoggedInUserAsync();
            var preferenceMisMatch =
                gameNightDetails.FoodOptions.All(f => currentUser.Preferences.All(fp => fp.Id != f.Id));
            if (preferenceMisMatch)
            {
                TempData["PreferenceError"] = "One or more of your preferences is not present.";
            }
            
            return View(new GameNightDetailsModel() { GameNight = gameNightDetails, CurrentUser = currentUser});
        }

        public async Task<IActionResult> Attending()
        {
            var gameNightList = await _gameNightService.GetAllAsync();
            var currentUser = await _userService.GetLoggedInUserAsync();

            return View("UserList", new GameNightListModel() { DisplayGameNights = gameNightList, CurrentUser = currentUser });
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
                    return RedirectToAction("GameNightDetails", new { gameNightId = gameNightId });
                }
                else
                {
                    //Handle case where user is already attending or game night is full
                    TempData["JoinGameNightError"] = "You are already attending this game night or it is full.";
                    return RedirectToAction("GameNightDetails", new { gameNightId = gameNightId });
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

        



        public async Task<IActionResult> Leave(int gameNightId)
        {
            // Get the current user's ID
            var identityUserKey = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identityUserKey != null)
            {
                //Call the service to join the game night
                var result = await _gameNightService.LeaveGameNightAsync(gameNightId, identityUserKey);

                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    //Handle case where user is already attending or game night is full
                    TempData["LeaveGameNightError"] = "Something went wrong when leaving... Try again";
                    return RedirectToAction("List");
                }
            }

            else
            {
                //Handle case where user is not authenticated
                TempData["LeaveGameNightError"] = "You must be logged in to leave a game night.";

                //TODO: Fix the redirection to login page
                //return RedirectToAction("Login", $"Identity/Account");

                //Temporary fix
                return RedirectToAction("List");

            }
        }
        /*
        ------------------------------------------------------------------------
        From this point on, the code is for the forms associated with GameNight
        ------------------------------------------------------------------------
        */

        //Step 1 of the GameNight Creation Process

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("List");
            }
            else
            {
                var games = await _gameService.GetAllAsync();
                shareableCrudGameNightModel = new() { CurrentUser = currentUser };
                return View(new CrudGameNightModel() { CurrentUser = currentUser, GameListModel = new GameListModel() { CurrentUser = currentUser, DisplayGames = games } });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CrudGameNightModel model)
        {
            if(ModelState.IsValid)
            {
                shareableCrudGameNightModel = model;
                //Redirect to next step in process
                return RedirectToAction("AddGames");
            }
            return View(model);
        }

        //Step 2 of the GameNight Creation Process

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddGames()
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("List");
            }
            else
            {
                var allGames = await _gameService.GetAllAsync();
                shareableCrudGameNightModel.GameListModel = new GameListModel() { CurrentUser = currentUser, DisplayGames = allGames };
                return View(shareableCrudGameNightModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Filter(CrudGameNightModel model)
        {
            var gameListModel = model.GameListModel;
            // Ensure selected genres and categories are also set
            gameListModel.SelectedGenres = gameListModel.SelectedGenres ?? new List<int>();
            gameListModel.SelectedCategories = gameListModel.SelectedCategories ?? new List<int>();

            // If the model state is valid, fetch games based on the filter criteria
            //GetAllAsync is overloaded, so we can pass the GameListModel to it
            var filteredGames = await _gameService.GetAllAsync(gameListModel);
            var currentUser = await _userService.GetLoggedInUserAsync();
            model.GameListModel.DisplayGames = filteredGames;
            model.GameListModel.CurrentUser = currentUser;

            // Return the filtered games to the view
            return View("AddGames", model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddGames(int[] SelectedGameIds)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("List");
            }

            shareableCrudGameNightModel.CurrentUser = currentUser;

            // Retrieve the selected games from the database based on IDs
            if (SelectedGameIds != null && SelectedGameIds.Length > 0)
            {
                //TODO Implement logic for getting list of IDs
                var selectedGames = await _gameService.GetAllByIdAsync(SelectedGameIds);
                shareableCrudGameNightModel.GameNight.Games = _mapper.Map<ICollection<Game>>(selectedGames);
            }

            // Redirect to the next step
            return RedirectToAction("AddFoodOptions");
        }

        //Step 3 of the GameNight Creation Process

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddFoodOptions()
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("List");
            }
            else
            {
                shareableCrudGameNightModel.CurrentUser = currentUser;
                var allFoodOptions = await _miscService.GetAllFoodOptionsAsync();
                shareableCrudGameNightModel.FoodOptions = allFoodOptions;

                shareableCrudGameNightModel.GameListModel = new GameListModel() { CurrentUser = currentUser};
                return View(shareableCrudGameNightModel);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFoodOptions(int[] SelectedFoodOptionsIds)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("List");
            }
            shareableCrudGameNightModel.CurrentUser = currentUser;

            // Retrieve the selected foodoption from the database based on IDs
            if (SelectedFoodOptionsIds != null && SelectedFoodOptionsIds.Length > 0)
            {
                //TODO Implement logic for getting list of IDs
                var selectedFoodOptions = await _miscService.GetAllFoodOptionDtoByIdAsync(SelectedFoodOptionsIds);
                shareableCrudGameNightModel.GameNight.FoodOptions = _mapper.Map<ICollection<FoodOptions>>(selectedFoodOptions);
            }
            shareableCrudGameNightModel.GameNight.OrganiserId = currentUser.Id;

            //Save model into DB
            //Delete Session data

            shareableCrudGameNightModel.GameListModel = null;
            _gameNightService.Insert(shareableCrudGameNightModel.GameNight);

            TempData["GameNightSuccess"] = "Game Night created successfully!";
            return RedirectToAction("List");
        }

        public async Task<IActionResult> AllTogether(CrudGameNightModel model, int[] SelectedGameIds)
        {
            if(SelectedGameIds.Length <= 0)
            { SelectedGameIds = new int[0]; }

            if (ModelState.IsValid)
            {
                //Add OrganiserId
                var currentUser = await _userService.GetLoggedInUserAsync();
                model.GameNight.OrganiserId = currentUser.Id;

                //Add GameIds to GameNight
                model.SelectedGameIds = SelectedGameIds;

                //Add FoodOptionIds to GameNight
                //model.SelectedFoodOptionIds = SelectedFoodOptionIds;

                var gamesFromDb = await _gameService.GetAllEntityAsync();
                model.GameNight.Games = gamesFromDb.Where(x => model.SelectedGameIds.Contains(x.Id)).ToList();

                _gameNightService.Insert(model.GameNight);
                TempData["GameNightSuccess"] = "Game Night created successfully!";
                return RedirectToAction("List");
            }
            var games = await _gameService.GetAllAsync();
            model.GameListModel.DisplayGames = games;
            return View("Create", model);
        }

            private async Task<string?> UploadPhotoToServerAsync(IFormFile toBeUploadedImage)
        {
            if (toBeUploadedImage != null)
            {
                var fileName = toBeUploadedImage.FileName.Trim().ToLower().Replace(" ", "");
                var folder = "img/gamenight/";
                folder += Guid.NewGuid().ToString() + "_" + fileName;
                //PathCombine considers the first argument as the root, if we add  / in front of the folder, it will be considered as the root!!! 
                var serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folder);

                using (var image = await Image.LoadAsync(toBeUploadedImage.OpenReadStream()))
                {
                    // Resize the image, maintaining aspect ratio
                    int maxWidth = 1200; // Set your desired maximum width here
                    int maxHeight = 800; // Set your desired maximum height here
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max, // Ensures it won't stretch
                        Size = new Size(maxWidth, maxHeight)
                    }));

                    // Save the resized image to the server folder
                    await image.SaveAsync(serverFolder);
                }
                //Add / to the folder, for the DB
                return "/" + folder;
            }
            return null;
        }

        private void DeleteOldImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return; // Nothing to delete if the URL is empty
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
