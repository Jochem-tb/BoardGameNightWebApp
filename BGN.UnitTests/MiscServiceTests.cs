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
    public class MiscServiceTests
    {
        private readonly IMiscService _sut;
        private readonly IMapper _mapper;
        private readonly IMiscRepository _miscRepository = Substitute.For<IMiscRepository>();
        private readonly IRepositoryManager _repositoryManager = Substitute.For<IRepositoryManager>();
        public MiscServiceTests()
        {
            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                // Add your mapping profiles here
                cfg.AddProfile<BGN.Services.Mapping.MappingProfile>();
            });
            _mapper = config.CreateMapper();
            // Initialize the SUT to use the mock service.
            _sut = new MiscService(_repositoryManager, _mapper);
            _repositoryManager.MiscRepository.Returns(_miscRepository);
        }

        [Fact]
        public async void GetAllFoodOptionsAsync_ShouldReturnFoodOptionsAsDto()
        {
            // Arrange
            var foodOptions = new List<FoodOptions>
            {
                new FoodOptions { Id = 1, Name = "Food Option 1" },
                new FoodOptions { Id = 2, Name = "Food Option 2" },
                new FoodOptions { Id = 3, Name = "Food Option 3" },
                new FoodOptions { Id = 4, Name = "Food Option 4" },
                new FoodOptions { Id = 5, Name = "Food Option 5" },
            };
            _miscRepository.GetAllFoodOptionsAsync().Returns(foodOptions);
            var expectedDtos = _mapper.Map<IEnumerable<FoodOptionDto>>(foodOptions);

            // Act
            var result = await _sut.GetAllFoodOptionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(foodOptions.Count, result.Count());

            result.Should().BeEquivalentTo(expectedDtos);
        }

        [Fact]
        public async void GetAllFoodOptionsEntityAsync_ShouldReturnFoodOptionsAsEntity()
        {
            // Arrange
            var foodOptions = new List<FoodOptions>
            {
                new FoodOptions { Id = 1, Name = "Food Option 1" },
                new FoodOptions { Id = 2, Name = "Food Option 2" },
                new FoodOptions { Id = 3, Name = "Food Option 3" },
                new FoodOptions { Id = 4, Name = "Food Option 4" },
                new FoodOptions { Id = 5, Name = "Food Option 5" },
            };
            _miscRepository.GetAllFoodOptionsAsync().Returns(foodOptions);

            // Act
            var result = await _sut.GetAllFoodOptionsEntityAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(foodOptions.Count, result.Count());

            result.Should().BeEquivalentTo(foodOptions);
        }
    }
}
