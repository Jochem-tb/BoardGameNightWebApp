using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services
{
    internal sealed class PersonService : IPersonService
    {
        private readonly IRepositoryManager _repositoryManager;
        public PersonService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

        public Task<PersonDto> CreateAsync(PersonDto person)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PersonDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PersonDto> GetByIdAsync(int id)
        {
            var dbPerson = await _repositoryManager.PersonRepository.GetByIdAsync(id);
            return new PersonDto()
            {
                FirstName = dbPerson.FirstName,
                LastName = dbPerson.LastName,
                Email = dbPerson.Email
            };

        }

        public Task<PersonDto> UpdateAsync(int id, PersonDto person)
        {
            throw new NotImplementedException();
        }
    }
}
