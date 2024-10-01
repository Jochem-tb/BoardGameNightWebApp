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
        private readonly Lazy<IGameNightRepository> _gameNightRepository;

        public RepositoryManager(RepositoryDbContext dbContext)
        {
            _personRepository = new Lazy<IPersonRepository>(() => new PersonRepository(dbContext));
            _gameNightRepository = new Lazy<IGameNightRepository>(() => new GameNightRepository(dbContext));
        }

        public IPersonRepository PersonRepository => _personRepository.Value;
        public IGameNightRepository GameNightRepository => _gameNightRepository.Value;

    }
}
