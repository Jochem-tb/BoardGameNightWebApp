using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGN.Domain.Repositories;
using BGN.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BGN.Infrastructure.Repositories
{
    internal sealed class PersonRepository : IPersonRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public PersonRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _dbContext.Persons.ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            Console.WriteLine("Getting person by id in repository");
            return await _dbContext.Persons.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Insert(Person person)
        {
            _dbContext.Persons.Add(person);
            _dbContext.SaveChanges();
        }

        public void Remove(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
