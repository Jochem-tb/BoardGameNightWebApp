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
using System.Text.Json;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace BGN.UI.Controllers
{
    [Authorize]
    public class GameNightController : Controller
    {
        private readonly IGameNightService _gameNightService;
        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        private readonly IMiscService _miscService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        private const string GAMENIGHT_SESSION_PERSISTENT_KEY = "GameNightModel";

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

            return View(new GameNightDetailsModel() { GameNight = gameNightDetails, CurrentUser = currentUser });
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

            if (identityUserKey != null)
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
            if (currentUser == null) return RedirectToAction("List");


            var model = new CrudGameNightModel()
            {
                CurrentUser = currentUser,
            };

            // Store initial model in Session
            HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(model));
            return View(model);

        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CrudGameNightModel model)
        {
            if (ModelState.IsValid)
            {
                //Save data in Session:
                var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY));
                exModel.GameNight = new GameNight()
                {
                    Name = model.GameNight.Name,
                    Date = model.GameNight.Date,
                    Time = model.GameNight.Time,
                    Organiser = null,
                    OrganiserId = exModel.CurrentUser.Id,
                    Street = model.GameNight.Street,
                    HouseNr = model.GameNight.HouseNr,
                    City = model.GameNight.City,
                    MaxPlayers = model.GameNight.MaxPlayers,
                    ImgUrl = model.GameNight.ImgUrl
                };

                HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));

                //Redirect to next step in process
                return RedirectToAction("AddGames");
            }
            return View(model);
        }

        //Step 2 of the GameNight Creation Process

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddGames(CrudGameNightModel model)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null) return RedirectToAction("List");

            var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY));

            var allGames = await _gameService.GetAllAsync();
            exModel.GameListModel = new GameListModel() { CurrentUser = currentUser, DisplayGames = allGames };

            HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));

            return View(exModel);

        }

        [HttpGet]
        public async Task<IActionResult> Filter(CrudGameNightModel model)
        {
            var gameListModel = model.GameListModel;
            // Ensure selected genres and categories are also set
            gameListModel.SelectedGenres = gameListModel.SelectedGenres ?? new List<int>();
            gameListModel.SelectedCategories = gameListModel.SelectedCategories ?? new List<int>();

            model.SelectedGameIds = model.SelectedGameIds;

            // If the model state is valid, fetch games based on the filter criteria
            //GetAllAsync is overloaded, so we can pass the GameListModel to it
            var filteredGames = await _gameService.GetAllAsync(gameListModel);
            var currentUser = await _userService.GetLoggedInUserAsync();
            model.GameListModel.DisplayGames = filteredGames;
            model.GameListModel.CurrentUser = currentUser;

            var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY));
            exModel.GameListModel = model.GameListModel;
            HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));

            // Return the filtered games to the view
            return View("AddGames", exModel);
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


            // Retrieve the selected games from the database based on IDs
            if (SelectedGameIds != null && SelectedGameIds.Length > 0)
            {
                //TODO Implement logic for getting list of IDs
                var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY));
                var allGameEntity = await _gameService.GetAllEntityAsync();
                var selectedGames = allGameEntity.Where(g => SelectedGameIds.Contains(g.Id)).ToList();
                exModel.GameNight.Games = selectedGames;
                HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));
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
            if (currentUser == null) return RedirectToAction("List");

            var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY));
            var allFoodOptions = await _miscService.GetAllFoodOptionsAsync();
            exModel.FoodOptions = allFoodOptions;
            HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));

            return View(exModel);
            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFoodOptions(CrudGameNightModel model ,int[] SelectedFoodOptionsIds)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null) return RedirectToAction("List");

            //Retreive gameNight from session
            var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY));

            // Retrieve the selected foodoption from the database based on IDs
            if (SelectedFoodOptionsIds != null && SelectedFoodOptionsIds.Length > 0)
            {
                //TODO Implement logic for getting list of IDs
                var allFoodOptionEntity = await _miscService.GetAllFoodOptionsEntityAsync();
                var selectedFoodOptions = allFoodOptionEntity.Where(f => SelectedFoodOptionsIds.Contains(f.Id)).ToList();
                exModel.GameNight.FoodOptions = selectedFoodOptions;
            }
            exModel.FoodOptions = null;
            model = null;

            _gameNightService.Insert(exModel.GameNight);

            TempData["GameNightCreateSuccess"] = "Game Night created successfully!";
            return RedirectToAction("List");
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
