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
    internal sealed class PersonRepository(RepositoryDbContext dbContext) : IPersonRepository
    {
        private readonly RepositoryDbContext _dbContext = dbContext;

        public void Insert(Person person)
        {
            _dbContext.AttachRange(person.Preferences);
            _dbContext.Persons.Add(person);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<Gender>> GetAllGendersAsync()
        {
            return await _dbContext.Genders.ToListAsync();
        }

        public async Task<Person?> GetPersonIdByUserKey(string userIdentityKey)
        {
            if (string.IsNullOrWhiteSpace(userIdentityKey))
            {
                return null!;
            }
            else
            {
                return await _dbContext.Persons
                    .Include(x => x.Gender)
                    .Include(x => x.Preferences)
                    .FirstOrDefaultAsync(x => x.IdentityUserId == userIdentityKey);
            }
        }
    }
}
