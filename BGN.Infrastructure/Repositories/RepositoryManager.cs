using BGN.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Infrastructure.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IPersonRepository> _personRepository;

        public RepositoryManager(RepositoryDbContext dbContext)
        {
            _personRepository = new Lazy<IPersonRepository>(() => new PersonRepository(dbContext));
        }

        public IPersonRepository PersonRepository => _personRepository.Value;

    }
}
