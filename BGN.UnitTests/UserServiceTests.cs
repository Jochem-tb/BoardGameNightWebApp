using AutoMapper;
using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using BGN.Services;
using BGN.Services.Abstractions;
using BGN.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BGN.UnitTests
{
    public class UserServiceTests
    {
        private readonly IUserService _sut;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager = Substitute.For<IRepositoryManager>();
        private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();
        private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        public UserServiceTests()
        {
            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                // Add your mapping profiles here
                cfg.AddProfile<BGN.Services.Mapping.MappingProfile>();
            });
            _mapper = config.CreateMapper();
            // Initialize the SUT to use the mock service.
            _sut = new UserService(_repositoryManager, _mapper, _httpContextAccessor);
           
        }

        [Fact]
        public async void GetLoggedInUserAsync_WhenUserIsLoggedIn_ReturnsPersonDto()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                GenderId = 1,
                Email = "xx@xx.nl",
                IdentityUserId = "1392872373-ad823ldkdd",
                Street = "Mainstreet",
                City = "New York",
                PostalCode = "1234AB",
                DateOfBirth = new DateTime(1990, 1, 1),
                HouseNr = "21a",
            };
            var identityUserKey = "1392872373-ad823ldkdd";

            // Create a mock ClaimsPrincipal
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, identityUserKey)
                };
            var claimsIdentity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Set up the HttpContextAccessor
            var httpContext = new DefaultHttpContext { User = claimsPrincipal };
            _httpContextAccessor.HttpContext.Returns(httpContext);

            // Set up the repository mock
            _repositoryManager.PersonRepository.GetPersonIdByUserKey(identityUserKey).Returns(person);

            var personDto = _mapper.Map<PersonDto>(person);

            // Act
            var result = await _sut.GetLoggedInUserAsync();

            // Assert
            result.Should().BeEquivalentTo(personDto);
        }
    }
}
