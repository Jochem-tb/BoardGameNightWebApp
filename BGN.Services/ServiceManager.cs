using BGN.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services
{
    internal sealed class ServiceManager : IServiceManager
    {
        // Lazy<T> is a thread-safe way to create a singleton
        //Lazy zorgt ervoor dat de service pas wordt aangemaakt als deze nodig is
        private readonly Lazy<IPersonService> _lazyPersonService;


        // Constructor to inject the repository manager

        //TODO: Implement the constructor
                //public ServiceManager(IRepositoryManager repositoryManager)
                //{
                 //    _lazyPersonService = new Lazy<IPersonService>(() => new PersonService(repositoryManager));
                 //}


        // Property to access the PersonService
        // This is the only way to access the service
        public IPersonService PersonService => _lazyPersonService.Value;
    }
}
