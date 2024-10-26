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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using BGN.Shared;

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
            return View(new GameNightListModel() { DisplayGameNights = gameNightList, CurrentUser = currentUser! });
        }

        public async Task<IActionResult> GameNightDetails(int gameNightId)
        {
            var gameNightDetails = await _gameNightService.GetByIdAsync(gameNightId);
            var currentUser = await _userService.GetLoggedInUserAsync();
            var allGameNights = await _gameNightService.GetAllAsync();

            var showNoShowDictionary = new Dictionary<int, float>();
            foreach (var member in gameNightDetails.Attendees)
            {
                var attendee = member.Attendee;
                var attendeeGameNights = allGameNights.Where(gn => gn.Attendees.Any(a => a.Attendee.Id == attendee.Id && a.AttendanceStatus != null));
                var totalAttended = attendeeGameNights.Count();
                var attendedWithShow = attendeeGameNights.Count(gn => gn.Attendees.Any(a => a.Attendee.Id == attendee.Id && a.AttendanceStatus == true));
                // Calculate the percentage of shows
                float percentageShow = totalAttended > 0 ? (attendedWithShow / (float)totalAttended) * 100 : 0;
                percentageShow = (float)Math.Round(percentageShow, 1); // Round to 1 decimal place

                showNoShowDictionary.Add(attendee.Id, percentageShow);
            }

            var preferenceMisMatch = currentUser!.Preferences.All(preference =>
                    gameNightDetails.FoodOptions.Any(option => option.Id == preference.Id));
            if (!preferenceMisMatch)
            {
                TempData["PreferenceError"] = "One or more of your preferences is not present.";
            }

            return View(new GameNightDetailsModel() { GameNight = gameNightDetails, CurrentUser = currentUser, ShowNoShowDictionary = showNoShowDictionary });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAttendance(GameNightDetailsModel model)
        {
            var existingGameNight = await _gameNightService.GetByIdEntityAsync(model.GameNight.Id);
            foreach (var member in existingGameNight!.Attendees)
            {
                var attendee = member.Attendee;
                var attendanceStatus = Request.Form[$"attendance_{attendee.Id}"];
                bool isPresent = attendanceStatus.ToString().Equals("show");

                member.AttendanceStatus = isPresent; 
            }
            _gameNightService.UpdateAttendance(existingGameNight);

            return RedirectToAction("GameNightDetails", new { gameNightId = model.GameNight.Id });
        }

        public async Task<IActionResult> Attending()
        {
            var gameNightList = await _gameNightService.GetAllAsync();
            var currentUser = await _userService.GetLoggedInUserAsync();

            return View("UserList", new GameNightListModel() { DisplayGameNights = gameNightList, CurrentUser = currentUser! });
        }

        [HttpGet]
        public async Task<IActionResult> FilterList(GameNightListModel model)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            model.CurrentUser = currentUser!;

            model.SelectedFoodOptions ??= new List<int>();

            var filteredGameNights = await _gameNightService.GetAllAsync(model);
            model.DisplayGameNights = filteredGameNights;

            // Return the filtered games to the view
            return View("List", model);

        }

        /*
        ------------------------------------------------------------------------
        From this point, you find the JOIN/LEAVE implemenentations for GameNight
        ------------------------------------------------------------------------
        */

        [Authorize]
        public async Task<IActionResult> Join(int gameNightId, bool CheckForMismatch = true)
        {
            //Get the current user's ID
            var identityUserKey = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identityUserKey != null)
            {
                //Check for mismatch in FoodOptions and Preferences

                if (CheckForMismatch)
                {
                    var currentUser = await _userService.GetLoggedInUserAsync();
                    var gameNightDetails = await _gameNightService.GetByIdAsync(gameNightId);

                    var preferenceMisMatch = currentUser!.Preferences.All(preference =>
                        gameNightDetails.FoodOptions.Any(option => option.Id == preference.Id));

                    if (!preferenceMisMatch)
                    {
                        TempData["PreferenceMisMatchError"] = "One or more of your preferences is not present.";
                        return RedirectToAction("GameNightDetails", new { gameNightId });
                    }
                }

                //Call the service to join the game night
                var result = await _gameNightService.JoinGameNightAsync(gameNightId, identityUserKey);

                if(result == null)
                {
                    //Handle case where user is already attending a gamenight on the same Date
                    TempData["JoinGameNightError"] = "You are already attending a game night on this day.";
                    return RedirectToAction("GameNightDetails", new { gameNightId });
                }
                else if ((bool)result)
                {
                    //Success!
                    return RedirectToAction("GameNightDetails", new { gameNightId });
                }
                else
                {
                    //Handle case where user is already attending or game night is full
                    TempData["JoinGameNightError"] = "You are already attending this game night or it is full.";
                    return RedirectToAction("GameNightDetails", new { gameNightId });
                }
            }

            else
            {
                //Handle case where user is not authenticated
                TempData["JoinGameNightError"] = "You must be logged in to join a game night.";

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

                return RedirectToAction("List");

            }
        }
        /*
        ------------------------------------------------------------------------
        From this point on, the code is for the forms associated with GameNight
        ------------------------------------------------------------------------
        */

        /*
        ------------------------------------------------------------------------
        Here you find the CRUD operations for GameNight (Create, Edit, Delete)
        ------------------------------------------------------------------------
        */

        /*
        ----------------------------------------
        Step 1 of the GameNight Creation Process
        ----------------------------------------
        */

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null) return RedirectToAction("List");


            var model = new CrudGameNightModel()
            {
                CurrentUser = currentUser
            };

            // Store initial model in Session
            HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(model));
            ViewData["Title"] = "Organize new Gamenight";
            return View(model);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int gameNightId)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null) return RedirectToAction("List");

            var result = await _gameNightService.DeleteByIdAsync(gameNightId, currentUser.IdentityUserId);
            if(result)
            {
                TempData["GameNightDeleteSuccess"] = "Game Night successfully deleted!";
                return RedirectToAction("List");
            }
            else
            {
                TempData["GameNightDeleteError"] = "Something went wrong when deleting the Game Night";
                return RedirectToAction("List");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int gameNightId)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null) return RedirectToAction("List");

            var existingGameNight = await _gameNightService.GetByIdAsync(gameNightId);

            //If anyone is attending that is not the organiser, redirect to list
            if(existingGameNight.Attendees.Any(a => a.Attendee.Id != currentUser.Id))
            {
                TempData["EditError"] = "You can't edit while people are already attending";
                return RedirectToAction("GameNightDetails", new { gameNightId });
            }

            var model = new CrudGameNightModel
            {
                CurrentUser = currentUser,
                GameNight = new GameNight()
                {
                    Id = existingGameNight.Id,
                    Name = existingGameNight.Name,
                    Date = existingGameNight.Date,
                    Time = existingGameNight.Time,
                    Street = existingGameNight.Street,
                    HouseNr = existingGameNight.HouseNr,
                    City = existingGameNight.City,
                    MaxPlayers = existingGameNight.MaxPlayers,
                    OnlyAdultWelcome = existingGameNight.OnlyAdultWelcome,
                    OrganiserId = existingGameNight.OrganiserId,
                    Organiser = null!,
                    ImgUrl = existingGameNight.ImgUrl,
                    Games = _mapper.Map<ICollection<Game>>(existingGameNight.Games),
                    FoodOptions = _mapper.Map<ICollection<FoodOptions>>(existingGameNight.FoodOptions)
                },
                isEditMode = true
            };

            // Store initial model in Session
            HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(model));
            ViewData["Title"] = "Edit Gamenight";
            return View("Create", model);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CrudGameNightModel model)
        {
            if (ModelState.IsValid)
            {
                //Save data in Session:
                var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY)!);
                if(exModel == null)
                {
                    return RedirectToAction("List");
                }
                if (exModel.isEditMode)
                {
                    exModel.GameNight!.Name = model.GameNight!.Name;
                    exModel.GameNight.Date = model.GameNight.Date;
                    exModel.GameNight.Time = model.GameNight.Time;
                    exModel.GameNight.Organiser = null!;
                    exModel.GameNight.OrganiserId = exModel.CurrentUser.Id;
                    exModel.GameNight.Street = model.GameNight.Street;
                    exModel.GameNight.HouseNr = model.GameNight.HouseNr;
                    exModel.GameNight.City = model.GameNight.City;
                    exModel.GameNight.MaxPlayers = model.GameNight.MaxPlayers;
                    exModel.GameNight.OnlyAdultWelcome = model.GameNight.OnlyAdultWelcome;
                    //Not assigning the ImgUrl, we keep the old
                    //Possible change hereunder
                }
                else
                {
                    exModel.GameNight = new GameNight()
                    {
                        Name = model.GameNight!.Name,
                        Date = model.GameNight.Date,
                        Time = model.GameNight.Time,
                        Organiser = null!,
                        OrganiserId = exModel.CurrentUser.Id,
                        Street = model.GameNight.Street,
                        HouseNr = model.GameNight.HouseNr,
                        City = model.GameNight.City,
                        MaxPlayers = model.GameNight.MaxPlayers,
                        OnlyAdultWelcome = model.GameNight.OnlyAdultWelcome,
                        ImgUrl = null!
                    };
                }


                // Check if a new cover photo is uploaded
                if (model.CoverPhoto != null && model.CoverPhoto.Length > 0)
                {
                    // Upload new photo and get the new URL
                    var newImageUrl = await UploadPhotoToServerAsync(model.CoverPhoto);

                    if (!string.IsNullOrEmpty(newImageUrl))
                    {
                        // Delete the old image if it exists
                        DeleteOldImage(exModel.GameNight.ImgUrl!);

                        // Set the new URL in the model
                        exModel.GameNight.ImgUrl = newImageUrl;
                    }
                }

                HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));

                //Redirect to next step in process
                return RedirectToAction("AddGames");
            }
            return View(model);
        }


        /*
        ----------------------------------------
        Step 2 of the GameNight Creation Process
        ----------------------------------------
        */

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddGames()
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null) return RedirectToAction("List");

            var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY)!);

            var allGames = await _gameService.GetAllAsync();
            exModel!.GameListModel = new GameListModel() { CurrentUser = currentUser, DisplayGames = allGames };

            HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));

            return View(exModel);

        }

        [HttpGet]
        public async Task<IActionResult> Filter(CrudGameNightModel model)
        {
            // Ensure selected genres and categories are also set
            model.GameListModel!.SelectedGenres = model.GameListModel.SelectedGenres ?? new List<int>();
            model.GameListModel!.SelectedCategories = model.GameListModel.SelectedCategories ?? new List<int>();

            // If the model state is valid, fetch games based on the filter criteria
            //GetAllAsync is overloaded, so we can pass the GameListModel to it
            var filteredGames = await _gameService.GetAllAsync(model.GameListModel);
            var currentUser = await _userService.GetLoggedInUserAsync();
            model.GameListModel.DisplayGames = filteredGames;
            model.GameListModel.CurrentUser = currentUser;

            var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY)!);
            exModel!.GameListModel = model.GameListModel;
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
                var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY)!);
                var allGameEntity = await _gameService.GetAllEntityAsync();
                var selectedGames = allGameEntity.Where(g => SelectedGameIds.Contains(g.Id)).ToList();
                exModel!.GameNight!.Games = selectedGames;
                HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));
            }

            // Redirect to the next step
            return RedirectToAction("AddFoodOptions");
        }

        /*
        ----------------------------------------
        //Step 3 of the GameNight Creation Process
        ----------------------------------------
        */

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddFoodOptions()
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null) return RedirectToAction("List");

            var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY)!);
            var allFoodOptions = await _miscService.GetAllFoodOptionsAsync();
            exModel!.FoodOptions = allFoodOptions;
            HttpContext.Session.SetString(GAMENIGHT_SESSION_PERSISTENT_KEY, JsonSerializer.Serialize(exModel));

            return View(exModel);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFoodOptions(int[] SelectedFoodOptionsIds)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            if (currentUser == null) return RedirectToAction("List");

            //Retreive gameNight from session
            var exModel = JsonSerializer.Deserialize<CrudGameNightModel>(HttpContext.Session.GetString(GAMENIGHT_SESSION_PERSISTENT_KEY)!);

            // Retrieve the selected foodoption from the database based on IDs
            if (SelectedFoodOptionsIds != null && SelectedFoodOptionsIds.Length > 0)
            {
                var allFoodOptionEntity = await _miscService.GetAllFoodOptionsEntityAsync();
                var selectedFoodOptions = allFoodOptionEntity.Where(f => SelectedFoodOptionsIds.Contains(f.Id)).ToList();
                exModel!.GameNight!.FoodOptions = selectedFoodOptions;
            }

            //Make distiction about putting new GameNight in DB or Update
            if (exModel!.isEditMode)
            {
                await _gameNightService.UpdateAsync(exModel.GameNight!);
                TempData["GameNightUpdateSuccess"] = "Game Night successfully updated!";
            }
            else
            {
                _gameNightService.Insert(exModel.GameNight!);
                TempData["GameNightCreateSuccess"] = "Game Night created successfully!";
            }

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

        private static void DeleteOldImage(string imageUrl)
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
