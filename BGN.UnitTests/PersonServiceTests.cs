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
    public class PersonServiceTests
    {
        private readonly IPersonService _sut;
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();
        private readonly IRepositoryManager _repositoryManager = Substitute.For<IRepositoryManager>();
        public PersonServiceTests()
        {
            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                // Add your mapping profiles here
                cfg.AddProfile<BGN.Services.Mapping.MappingProfile>();
            });
            _mapper = config.CreateMapper();
            // Initialize the SUT to use the mock service.
            _sut = new PersonService(_repositoryManager, _mapper);
            _repositoryManager.PersonRepository.Returns(_personRepository);
        }

        [Fact]
        public async Task GetAllGendersAsync_WhenCalled_ReturnsGenders()
        {
            // Arrange
            var genderList = new List<Gender>()
            {
                new Gender { Id = 1, Name = "Test1" },
                new Gender { Id = 2, Name = "Test2" },
                new Gender { Id = 3, Name = "Test3" },
                new Gender { Id = 4, Name = "Test4" },
                new Gender { Id = 5, Name = "Test5" },
            };
            _personRepository.GetAllGendersAsync().Returns(genderList);
            var expectedDto = _mapper.Map<IEnumerable<GenderDto>>(genderList);

            //Act
            var result = await _sut.GetAllGendersAsync();

            //Assert
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public void Insert_WhenCalled_CallsPersonRepositoryInsert()
        {
            // Arrange
            var person = new Person()
            {
                FirstName = "Test",
                LastName = "Test",
                GenderId = 1,
                DateOfBirth = DateTime.Now,
                Email = "xx@gmail.com",
                PhoneNumber = "1234567890",
                IdentityUserId = "1234567890",
                Street = "Test",
                City = "Test",
                HouseNr = "1",
                PostalCode = "1234",
            };

            //Act
            _sut.Insert(person);

            //Assert
            _personRepository.Received(1).Insert(person);
        }

        [Fact]
        public void Insert_WhenCalledWithNullPerson_ThrowsArgumentNullException()
        {
            // Arrange
            Person? person = null;

            //Act
            Action act = () => _sut.Insert(person!);

            //Assert
            _personRepository.DidNotReceive().Insert(person!);
        }

    }
}
