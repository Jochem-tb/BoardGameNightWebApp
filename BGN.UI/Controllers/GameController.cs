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

        [HttpPost]
        public async Task<IActionResult> Create(CrudGameModel model)
        {
            //var genre = await _gameService.GetGenreByIdAsync(model.Game.GenreId);
            //var category = await _gameService.GetCategoryByIdAsync(model.Game.CategoryId);

            //model.Game.Category = _mapper.Map<Category>(category);
            //model.Game.Genre = _mapper.Map<Genre>(genre);


            // Try to validate the Game object separately
            if (!ModelState.IsValid)
            {
                // Return the view with validation errors for the Game object
                return View();
            }
            else
            {
                var imgUrl = await UploadPhotoToServerAsync(model.CoverPhoto);
                
                //if (model.CoverPhoto != null)
                //{
                //    // Save the cover photo to the wwwroot/img/game folder
                //    var folder = "/img/game/";
                //    folder += Guid.NewGuid().ToString() + "_" + model.CoverPhoto.FileName;
                //    var serverFolder = Path.Combine(_hostEnvironment.WebRootPath, folder);
                //    using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                //    {
                //        await model.CoverPhoto.CopyToAsync(fileStream);
                //    }
                //    model.Game.ImgUrl = folder;
                //}

                if(!imgUrl.IsNullOrEmpty())
                {
                    model.Game.ImgUrl = imgUrl; // Save the URL in the database
                }

                
                // Insert the game into the database
                _gameService.Insert(model.Game!);
                TempData["CreateGameMessage"] = "Game created successfully!";
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
                //TODO Fix problem with serverfOLDER
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
                return "/"+folder;
            }
            return null;
        }
    }
}
