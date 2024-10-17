using AutoMapper;
using BGN.Domain.Entities;
using BGN.Services;
using BGN.Services.Abstractions;
using BGN.Shared;
using BGN.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using static NuGet.Packaging.PackagingConstants;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System.IO;


namespace BGN.UI.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IGameNightService _gameNightService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public GameController(IServiceManager serviceManager, IUserService userService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _gameService = serviceManager.GameService;
            _gameNightService = serviceManager.GameNightService;
            _userService = userService;
            _mapper = mapper;
            _hostEnvironment = webHostEnvironment;
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
            if (gameDetails == null)
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
            return View("List", new GameListModel()
            {
                DisplayGames = games, 
                SelectedGenres = new List<int>() { genId },
                CurrentUser = currentUser
            });
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
            var currentUser = await _userService.GetLoggedInUserAsync();
            gameListModel.DisplayGames = filteredGames;
            gameListModel.CurrentUser = currentUser;

            // Return the filtered games to the view
            return View("List", gameListModel);
        }

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
                return View(new CrudGameModel() { CurrentUser = currentUser });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int gameId)
        {
            var currentUser = await _userService.GetLoggedInUserAsync();
            var gameDto = await _gameService.GetByIdAsync(gameId);
            var game = _mapper.Map<Game>(gameDto);

            //Ensure both objects not null
            if (currentUser == null || game == null)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View(new CrudGameModel() { CurrentUser = currentUser, Game = game });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CrudGameModel model)
        {
            // Try to validate the Game object
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors for the Game object
                return View();
            }
            else
            {
                var imgUrl = await UploadPhotoToServerAsync(model.CoverPhoto);
                try
                {
                    if (!imgUrl.IsNullOrEmpty())
                    {
                        model.Game.ImgUrl = imgUrl; // Save the URL in the database
                    }

                    if (model.CurrentUser == null)
                    {
                        model.CurrentUser = await _userService.GetLoggedInUserAsync();
                        model.Game.OwnerId = model.CurrentUser.Id;
                    }



                    // Insert the game into the database
                    _gameService.Insert(model.Game!);
                    TempData["CreateGameMessage"] = "Game created successfully!";
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    DeleteOldImage(imgUrl);
                    TempData["UpdateGameError"] = "Something went wrong, while updating this game";
                    return RedirectToAction("List");
                }
                
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CrudGameModel model)
        {
            // Try to validate the Game object
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors for the Game object
                return View();
            }
            else
            {
                // Check if a new cover photo is uploaded
                if (model.CoverPhoto != null && model.CoverPhoto.Length > 0)
                {
                    // Upload new photo and get the new URL
                    var newImageUrl = await UploadPhotoToServerAsync(model.CoverPhoto);

                    if (!string.IsNullOrEmpty(newImageUrl))
                    {
                        // Delete the old image if it exists
                        DeleteOldImage(model.Game.ImgUrl);

                        // Set the new URL in the model
                        model.Game.ImgUrl = newImageUrl;
                    }
                }

                //Update game in the DB
                await _gameService.UpdateAsync(model.Game!);
                TempData["UpdateGameMessage"] = "Game successfully updated!";
                return RedirectToAction("List");
            }
        }

        private async Task<string?> UploadPhotoToServerAsync(IFormFile toBeUploadedImage)
        {
            if (toBeUploadedImage != null)
            {
                var fileName = toBeUploadedImage.FileName.Trim().ToLower().Replace(" ","");
                var folder = "img/game/";
                folder += Guid.NewGuid().ToString() + "_" + fileName;
                //PathCombine considers the first argument as the root, if we add  / in front of the folder, it will be considered as the root!!! 
                var serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folder);

                using (var image = await Image.LoadAsync(toBeUploadedImage.OpenReadStream()))
                {
                    // Resize the image, maintaining aspect ratio
                    int maxWidth = 800; // Set your desired maximum width here
                    int maxHeight = 400; // Set your desired maximum height here
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max, // Ensures it won't stretch
                        Size = new Size(maxWidth, maxHeight)
                    }));

                    // Save the resized image to the server folder
                    await image.SaveAsync(serverFolder);
                }
                //Add / to the folder, for the DB
                return "/" +folder;
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
