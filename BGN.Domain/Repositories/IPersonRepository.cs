using BGN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        void Insert(Person person);
        void Remove(Person person);

        Task<IEnumerable<Gender>> GetAllGendersAsync();

        Task<Person> GetPersonIdByUserKey(string userIdentityKey);
    }
}
