using AutoMapper;
using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        // Lazy<T> is a thread-safe way to create a singleton
        //Lazy zorgt ervoor dat de service pas wordt aangemaakt als deze nodig is
        private readonly IMapper _mapper;
        private readonly Lazy<IPersonService> _lazyPersonService;
        private readonly Lazy<IGameNightService> _lazyGameNightService;
        private readonly Lazy<IGameService> _lazyGameService;


        // Constructor to inject the repository manager

        //TODO: Implement the constructor
        public ServiceManager(IMapper mapper, IRepositoryManager repositoryManager)
        {
            _mapper = mapper;
            _lazyPersonService = new Lazy<IPersonService>(() => new PersonService(repositoryManager));
            _lazyGameNightService = new Lazy<IGameNightService>(() => new GameNightService(repositoryManager, _mapper));
            _lazyGameService = new Lazy<IGameService>(() => new GameService(repositoryManager, _mapper));
        }


        // Property to access the PersonService
        // This is the only way to access the service
        public IPersonService PersonService => _lazyPersonService.Value;
        public IGameNightService GameNightService => _lazyGameNightService.Value;
        public IGameService GameService => _lazyGameService.Value;
    }
}
