using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BGN.Domain.Entities;
using AutoMapper;
using Humanizer;
using BGN.Shared;
using BGN.Services.Mapping;
using FluentAssertions;
using BGN.Services.Abstractions.FilterModels;
using BGN.UI.Models;

namespace BGN.UnitTests
{
    public class GameServiceTests
    {
        private readonly IGameService _sut;
        private readonly IMapper _mapper;
        private readonly IGameRepository _gameRepository = Substitute.For<IGameRepository>();
        private readonly IRepositoryManager _repositoryManager = Substitute.For<IRepositoryManager>();

        public GameServiceTests()
        {
            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                // Add your mapping profiles here
                cfg.AddProfile<BGN.Services.Mapping.MappingProfile>();
            });
            _mapper = config.CreateMapper();
            // Initialize the SUT to use the mock service.
            _sut = new GameService(_repositoryManager, _mapper);
            _repositoryManager.GameRepository.Returns(_gameRepository);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnAllGamesAsDto()
        {
            //Arrange
            var games = new List<Game>
            {
                new() { Name = "name1",
                    Category = new Category {Name="kaart" },
                    CategoryId = 1,
                    Genre = new Genre{Name="coop" },
                    GenreId=1,
                    MinPlayers=0,
                    MaxPlayers=5,
                    OwnerId=208,
                    ImgUrl="url" },
                new() { Name = "name2",
                    Category = new Category {Name="bord" },
                    CategoryId = 2,
                    Genre = new Genre{Name="horror" },
                    GenreId=2,
                    MinPlayers=2,
                    MaxPlayers=10,
                    OwnerId=218,
                    ImgUrl="url" },
                new() { Name = "name3",
                    Category = new Category {Name="roleplay" },
                    CategoryId = 5,
                    Genre = new Genre{Name="party" },
                    GenreId=3,
                    MinPlayers=6,
                    MaxPlayers=12,
                    OwnerId=102,
                    ImgUrl="url" },
            };
            _repositoryManager.GameRepository.GetAllAsync().Returns(games);

            // Expected mapped DTOs
            var expectedDtos = _mapper.Map<IEnumerable<GameDto>>(games);

            //Act
            var sutGames = await _sut.GetAllAsync();

            // Assert
            Assert.Equal(games.Count, sutGames.Count());

            //using Fluent Assertions
            sutGames.Should().BeEquivalentTo(expectedDtos);
        }

        [Fact]
        public async void GetAllCategories_ShouldReturnAllCategoriesAsDto()
        {
            //Arrange
            var categories = new List<Category>
            {
                new() {Id = 1,
                Name = "cat1"},
                new() {Id = 2,
                Name = "cat2"},
                new() {Id = 3,
                Name = "cat3"},
                new() {Id = 4,
                Name = "cat4"},
                new() {Id = 5,
                Name = "cat5"},
                };
            _repositoryManager.GameRepository.GetAllCategoriesAsync().Returns(categories);

            // Expected mapped DTOs
            var expectedDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            //Act
            var sutCategories = await _sut.GetAllCategoriesAsync();

            // Assert
            Assert.Equal(categories.Count, sutCategories.Count());

            //using Fluent Assertions
            sutCategories.Should().BeEquivalentTo(expectedDtos);
        }

        [Fact]
        public async void GetAllGameByCategoryIdAsync_ShouldReturnAllGamesWithCategoryIdAsDto()
        {
            //Arrange
            var games = new List<Game>
            {
                new() { Name = "name1",
                    Category = new Category {Id = 1, Name="kaart" },
                    CategoryId = 1,
                    Genre = new Genre{Id=1, Name="coop" },
                    GenreId=1,
                    MinPlayers=0,
                    MaxPlayers=5,
                    OwnerId=208,
                    ImgUrl="url" },
                new() { Name = "name2",
                    Category = new Category {Id=2,Name="bord" },
                    CategoryId = 2,
                    Genre = new Genre{Id=2,Name="horror" },
                    GenreId=2,
                    MinPlayers=2,
                    MaxPlayers=10,
                    OwnerId=218,
                    ImgUrl="url" },
                new() { Name = "name3",
                    Category = new Category {Id=5,Name="roleplay" },
                    CategoryId = 5,
                    Genre = new Genre{Id=3,Name="party" },
                    GenreId=3,
                    MinPlayers=6,
                    MaxPlayers=12,
                    OwnerId=102,
                    ImgUrl="url" },
            };
            var catId = 5;
            _repositoryManager.GameRepository.GetAllGameByCategoryIdAsync(catId).Returns(games.Where(g => g.Category.Id == catId));

            var expectedDtos = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.Category.Id == catId));

            //Act
            var sutGames = await _sut.GetAllGameByCategoryIdAsync(catId);

            // Assert
            Assert.NotEqual(games.Count, sutGames.Count());

            //using Fluent Assertions
            sutGames.Should().BeEquivalentTo(expectedDtos);
        }

        [Fact]
        public async void GetAllGameByGenreIdAsync_ShouldReturnAllGamesWithGenreIdAsDto()
        {
            //Arrange
            var games = new List<Game>
            {
                new() { Name = "name1",
                    Category = new Category {Id = 1, Name="kaart" },
                    CategoryId = 1,
                    Genre = new Genre{Id=1, Name="coop" },
                    GenreId=1,
                    MinPlayers=0,
                    MaxPlayers=5,
                    OwnerId=208,
                    ImgUrl="url" },
                new() { Name = "name2",
                    Category = new Category {Id=2,Name="bord" },
                    CategoryId = 2,
                    Genre = new Genre{Id=2,Name="horror" },
                    GenreId=2,
                    MinPlayers=2,
                    MaxPlayers=10,
                    OwnerId=218,
                    ImgUrl="url" },
                new() { Name = "name3",
                    Category = new Category {Id=5,Name="roleplay" },
                    CategoryId = 5,
                    Genre = new Genre{Id=3,Name="party" },
                    GenreId=3,
                    MinPlayers=6,
                    MaxPlayers=12,
                    OwnerId=102,
                    ImgUrl="url" },
            };
            var genId = 2;
            _repositoryManager.GameRepository.GetAllGameByGenreIdAsync(genId).Returns(games.Where(g => g.Genre.Id == genId));

            var expectedDtos = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.Genre.Id == genId));

            //Act
            var sutGames = await _sut.GetAllGameByGenreIdAsync(genId);

            // Assert
            Assert.NotEqual(games.Count, sutGames.Count());

            //using Fluent Assertions
            sutGames.Should().BeEquivalentTo(expectedDtos);
        }

        /* 
         * ------------
         *  Harder tests
         *  Filter tests
         *  ------------
         */

        [Fact]
        public async void GetAllAsyncWithFilter_MinPlayer_And_MaxPlayer_Double()
        {
            //Arrange
            var games = new List<Game>
            {
                new() { Name = "name1",
                    Category = new Category {Name="kaart" },
                    CategoryId = 1,
                    Genre = new Genre{Name="coop" },
                    GenreId=1,
                    MinPlayers=0,
                    MaxPlayers=5,
                    OwnerId=208,
                    ImgUrl="url" },
                new() { Name = "name2",
                    Category = new Category {Name="bord" },
                    CategoryId = 2,
                    Genre = new Genre{Name="horror" },
                    GenreId=2,
                    MinPlayers=2,
                    MaxPlayers=10,
                    OwnerId=218,
                    ImgUrl="url" },
                new() { Name = "name3",
                    Category = new Category {Name="roleplay" },
                    CategoryId = 5,
                    Genre = new Genre{Name="party" },
                    GenreId=3,
                    MinPlayers=6,
                    MaxPlayers=12,
                    OwnerId=102,
                    ImgUrl="url" },
            };

            var filterModelMinPlayer = new GameListModel()
            {
                CurrentUser = null!,
                SearchMinPlayers = 2,
                SelectedGenres = new List<int>(),
                SelectedCategories = new List<int>()
            };
            var filterModelMaxPlayer = new GameListModel()
            {
                CurrentUser = null!,
                SearchMaxPlayers = 5,
                SelectedGenres = new List<int>(),
                SelectedCategories = new List<int>()
            };
            _repositoryManager.GameRepository.GetAllGamesAsQueryableAsync().Returns(games.AsQueryable());

            // Expected mapped DTOs
            var expectedDtosMin = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.MinPlayers >= filterModelMinPlayer.SearchMinPlayers));
            var expectedDtosMax = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.MaxPlayers <= filterModelMaxPlayer.SearchMaxPlayers));

            //Act
            var sutGamesMin = await _sut.GetAllAsync(filterModelMinPlayer);
            var sutGamesMax = await _sut.GetAllAsync(filterModelMaxPlayer);

            // Assert
            //using Fluent Assertions
            sutGamesMin.Should().BeEquivalentTo(expectedDtosMin);
            sutGamesMax.Should().BeEquivalentTo(expectedDtosMax);
        }

        [Fact]
        public async void GetAllAsyncWithFilter_SearchName()
        {
            //Arrange
            var games = new List<Game>
            {
                new() { Name = "name1",
                    Category = new Category {Name="kaart" },
                    CategoryId = 1,
                    Genre = new Genre{Name="coop" },
                    GenreId=1,
                    MinPlayers=0,
                    MaxPlayers=5,
                    OwnerId=208,
                    ImgUrl="url" },
                new() { Name = "name2",
                    Category = new Category {Name="bord" },
                    CategoryId = 2,
                    Genre = new Genre{Name="horror" },
                    GenreId=2,
                    MinPlayers=2,
                    MaxPlayers=10,
                    OwnerId=218,
                    ImgUrl="url" },
                new() { Name = "name3",
                    Category = new Category {Name="roleplay" },
                    CategoryId = 5,
                    Genre = new Genre{Name="party" },
                    GenreId=3,
                    MinPlayers=6,
                    MaxPlayers=12,
                    OwnerId=102,
                    ImgUrl="url" },
            };
            var filterModelName = new GameListModel()
            {
                CurrentUser = null!,
                SearchName = "name1",
                SelectedGenres = new List<int>(),
                SelectedCategories = new List<int>()
            };
            _repositoryManager.GameRepository.GetAllGamesAsQueryableAsync().Returns(games.AsQueryable());

            // Expected mapped DTOs
            var expectedDtos = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.Name.Contains(filterModelName.SearchName)));

            //Act
            var sutGames = await _sut.GetAllAsync(filterModelName);

            // Assert
            Assert.Equal(sutGames.Count(), expectedDtos.Count());

            //using Fluent Assertions
            sutGames.Should().BeEquivalentTo(expectedDtos);
        }

        [Fact]
        public async void GetAllAsyncWithFilter_IsAdultTrue_And_IsAdultFales_Double()
        {
            //Arrange
            var games = new List<Game>
            {
                new() { Name = "name1",
                    Category = new Category {Name="kaart" },
                    CategoryId = 1,
                    Genre = new Genre{Name="coop" },
                    GenreId=1,
                    MinPlayers=0,
                    MaxPlayers=5,
                    OwnerId=208,
                    IsAdult = true,
                    ImgUrl="url" },
                new() { Name = "name2",
                    Category = new Category {Name="bord" },
                    CategoryId = 2,
                    Genre = new Genre{Name="horror" },
                    GenreId=2,
                    MinPlayers=2,
                    MaxPlayers=10,
                    OwnerId=218,
                    IsAdult = true,
                    ImgUrl="url" },
                new() { Name = "name3",
                    Category = new Category {Name="roleplay" },
                    CategoryId = 5,
                    Genre = new Genre{Name="party" },
                    GenreId=3,
                    MinPlayers=6,
                    MaxPlayers=12,
                    OwnerId=102,
                    IsAdult = false,
                    ImgUrl="url" },
            };
            var filterModelIsAdultTrue = new GameListModel()
            {
                CurrentUser = null!,
                SearchIsAdult = true,
                SelectedGenres = new List<int>(),
                SelectedCategories = new List<int>()
            };
            var filterModelIsAdultFalse = new GameListModel()
            {
                CurrentUser = null!,
                SearchIsAdult = false,
                SelectedGenres = new List<int>(),
                SelectedCategories = new List<int>()
            };
            _repositoryManager.GameRepository.GetAllGamesAsQueryableAsync().Returns(games.AsQueryable());

            // Expected mapped DTOs
            var expectedDtosTrue = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.IsAdult.Equals(true)));
            var expectedDtosFalse = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.IsAdult.Equals(false)));


            //Act
            var sutGamesTrue = await _sut.GetAllAsync(filterModelIsAdultTrue);
            var sutGamesFalse = await _sut.GetAllAsync(filterModelIsAdultFalse);

            // Assert
            Assert.Equal(sutGamesTrue.Count(), expectedDtosTrue.Count());
            Assert.Equal(sutGamesFalse.Count(), expectedDtosFalse.Count());

            //using Fluent Assertions
            sutGamesTrue.Should().BeEquivalentTo(expectedDtosTrue);
            sutGamesFalse.Should().BeEquivalentTo(expectedDtosFalse);
        }

        [Fact]
        public async void GetAllAsyncWithFilter_MinTime_And_MaxTime_Double()
        {
            //Arrange
            var games = new List<Game>
            {
                new() { Name = "name1",
                    Category = new Category {Name="kaart" },
                    CategoryId = 1,
                    Genre = new Genre{Name="coop" },
                    GenreId=1,
                    MinPlayers=0,
                    MaxPlayers=5,
                    OwnerId=208,
                    IsAdult = true,
                    EstimatedTime = 50,
                    ImgUrl="url" },
                new() { Name = "name2",
                    Category = new Category {Name="bord" },
                    CategoryId = 2,
                    Genre = new Genre{Name="horror" },
                    GenreId=2,
                    MinPlayers=2,
                    MaxPlayers=10,
                    OwnerId=218,
                    IsAdult = true,
                    EstimatedTime = 90,
                    ImgUrl="url" },
                new() { Name = "name3",
                    Category = new Category {Name="roleplay" },
                    CategoryId = 5,
                    Genre = new Genre{Name="party" },
                    GenreId=3,
                    MinPlayers=6,
                    MaxPlayers=12,
                    OwnerId=102,
                    IsAdult = false,
                    EstimatedTime = 20,
                    ImgUrl="url" },
            };
            var filterModelLower = new GameListModel()
            {
                CurrentUser = null!,
                SearchEstimatedTimeLower = 30,
                SelectedGenres = new List<int>(),
                SelectedCategories = new List<int>()
            };
            var filterModelUpper = new GameListModel()
            {
                CurrentUser = null!,
                SearchEstimatedTimeUpper = 80,
                SelectedGenres = new List<int>(),
                SelectedCategories = new List<int>()
            };
            _repositoryManager.GameRepository.GetAllGamesAsQueryableAsync().Returns(games.AsQueryable());

            // Expected mapped DTOs
            var expectedDtosLower = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.EstimatedTime >= filterModelLower.SearchEstimatedTimeLower));
            var expectedDtosUpper = _mapper.Map<IEnumerable<GameDto>>(games.Where(g => g.EstimatedTime <= filterModelUpper.SearchEstimatedTimeUpper));


            //Act
            var sutGamesLower = await _sut.GetAllAsync(filterModelLower);
            var sutGamesUpper = await _sut.GetAllAsync(filterModelUpper);

            // Assert
            Assert.Equal(sutGamesLower.Count(), expectedDtosLower.Count());
            Assert.Equal(sutGamesUpper.Count(), expectedDtosUpper.Count());

            //using Fluent Assertions
            sutGamesLower.Should().BeEquivalentTo(expectedDtosLower);
            sutGamesUpper.Should().BeEquivalentTo(expectedDtosUpper);
        }
    }
}
