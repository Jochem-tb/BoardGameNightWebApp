using AutoMapper;
using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using BGN.Services;
using BGN.Services.Abstractions;
using BGN.Shared;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.UnitTests
{
    public class GameNightServiceTests
    {
        private readonly IGameNightService _sut;
        private readonly IMapper _mapper;
        private readonly IGameNightRepository _gameNightRepository = Substitute.For<IGameNightRepository>();
        private readonly IRepositoryManager _repositoryManager = Substitute.For<IRepositoryManager>();
        public GameNightServiceTests()
        {
            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                // Add your mapping profiles here
                cfg.AddProfile<BGN.Services.Mapping.MappingProfile>();
            });
            _mapper = config.CreateMapper();
            // Initialize the SUT to use the mock service.
            _sut = new GameNightService(_repositoryManager, _mapper);
            _repositoryManager.GameNightRepository.Returns(_gameNightRepository);
        }

        private List<GameNight> gameNights = new List<GameNight>
            {
                new GameNight
                {
                    Id = 1,
                    Date = DateTime.Now,
                    MaxPlayers = 10,
                    City = "City 1",
                    HouseNr = "12a",
                    Street = "Street 1",
                    ImgUrl = "https://www.google.com",
                    OrganiserId = 1,
                    Time = TimeSpan.FromHours(20),
                    Games = new List<Game>
                    {
                        new Game
                        {
                            Id = 1,
                            Name = "Game 1",
                            IsAdult = false,
                            Category = new Category
                            {
                                Id = 1,
                                Name = "Category 1"
                            },
                            CategoryId = 1,
                            MinPlayers = 2,
                            MaxPlayers = 4,
                            Genre = new Genre
                            {
                                Id = 1,
                                Name = "Genre 1"
                            },
                            GenreId = 1,
                            ImgUrl = "https://www.google.com",
                            Description = "Description 1",
                            OwnerId = 1,
                        }
                    },
                    Organiser = new Person
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        IdentityUserId = "1",
                        Email = "xx@xx.nl",
                        City = "City 1",
                        HouseNr = "12a",
                        PostalCode = "1234AB",
                        Street = "Street 1"
                    }
                },
                 new GameNight
                {
                    Id = 1,
                    Date = DateTime.Now,
                    MaxPlayers = 10,
                    City = "City 1",
                    HouseNr = "12a",
                    Street = "Street 1",
                    ImgUrl = "https://www.google.com",
                    OrganiserId = 1,
                    Time = TimeSpan.FromHours(20),
                    Games = new List<Game>
                    {
                        new Game
                        {
                            Id = 1,
                            Name = "Game 1",
                            IsAdult = false,
                            Category = new Category
                            {
                                Id = 1,
                                Name = "Category 1"
                            },
                            CategoryId = 1,
                            MinPlayers = 2,
                            MaxPlayers = 4,
                            Genre = new Genre
                            {
                                Id = 1,
                                Name = "Genre 1"
                            },
                            GenreId = 1,
                            ImgUrl = "https://www.google.com",
                            Description = "Description 1",
                            OwnerId = 1,
                        }
                    },
                    Organiser = new Person
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        IdentityUserId = "1",
                        Email = "xx@xx.nl",
                        City = "City 1",
                        HouseNr = "12a",
                        PostalCode = "1234AB",
                        Street = "Street 1"
                    }
                },
                  new GameNight
                {
                    Id = 1,
                    Date = DateTime.Now,
                    MaxPlayers = 10,
                    City = "City 1",
                    HouseNr = "12a",
                    Street = "Street 1",
                    ImgUrl = "https://www.google.com",
                    OrganiserId = 1,
                    Time = TimeSpan.FromHours(20),
                    Games = new List<Game>
                    {
                        new Game
                        {
                            Id = 1,
                            Name = "Game 1",
                            IsAdult = false,
                            Category = new Category
                            {
                                Id = 1,
                                Name = "Category 1"
                            },
                            CategoryId = 1,
                            MinPlayers = 2,
                            MaxPlayers = 4,
                            Genre = new Genre
                            {
                                Id = 1,
                                Name = "Genre 1"
                            },
                            GenreId = 1,
                            ImgUrl = "https://www.google.com",
                            Description = "Description 1",
                            OwnerId = 1,
                        }
                    },
                    Organiser = new Person
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        IdentityUserId = "1",
                        Email = "xx@xx.nl",
                        City = "City 1",
                        HouseNr = "12a",
                        PostalCode = "1234AB",
                        Street = "Street 1"
                    }
                }
            };

        [Fact]
        public async void GetAllAsync_ShouldReturnAllGameNights()
        {
            // Arrange
            var localGameNights = gameNights;
            _gameNightRepository.GetAllAsync().Returns(localGameNights);
            var expectedDto = _mapper.Map<IEnumerable<GameNightDto>>(localGameNights);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Count(), result.Count());
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnGameNightWithId()
        {
            // Arrange
            var localGameNights = gameNights;
            var id = 1;
            _gameNightRepository.GetByIdAsync(id).Returns(localGameNights.Where(gn => gn.Id == id).First());
            var expectedDto = _mapper.Map<GameNightDto>(localGameNights.Where(gn => gn.Id == id).First());

            // Act
            var result = await _sut.GetByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnNullWhenGameNightDoesNotExist()
        {
            // Arrange
            var localGameNights = gameNights;
            var id = 1;
            _gameNightRepository.GetByIdAsync(id).Returns((GameNight)null);

            // Act
            var result = await _sut.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetByIdEntityAsync_ShouldReturnGameNightWithId()
        {
            // Arrange
            var localGameNights = gameNights;
            var id = 1;
            _gameNightRepository.GetByIdAsync(id).Returns(localGameNights.Where(gn => gn.Id == id).First());
            var expectedDto = localGameNights.Where(gn => gn.Id == id).First();

            // Act
            var result = await _sut.GetByIdEntityAsync(id);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(localGameNights.Where(gn => gn.Id == id).First());
        }


    }
}
