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
        void Insert(Person person);

        Task<IEnumerable<Gender>> GetAllGendersAsync();

        Task<Person?> GetPersonIdByUserKey(string userIdentityKey);
    }
}
