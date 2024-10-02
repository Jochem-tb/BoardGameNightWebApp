using Microsoft.AspNetCore.Mvc;
using BGN.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using BGN.Services.Abstractions;
using BGN.Services;

namespace YourAppNamespace.ViewComponents
{
    public class GenreViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;

        public GenreViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var genres = await serviceManager; // Fetch genres from database
            return View(genres);  // Pass the genres to the view
        }
    }
}