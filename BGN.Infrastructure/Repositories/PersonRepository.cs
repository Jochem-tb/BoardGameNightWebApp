using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGN.Domain.Repositories;
using BGN.Domain.Entities;

namespace BGN.Infrastructure.Repositories
{
    internal sealed class PersonRepository : IPersonRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public PersonRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task<IEnumerable<Person>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Person person)
        {
            throw new NotImplementedException();
        }

        public void Remove(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
