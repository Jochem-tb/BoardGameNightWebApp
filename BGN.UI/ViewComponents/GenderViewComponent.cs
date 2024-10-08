using BGN.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.ViewComponents
{
    public class GenderViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;

        public GenderViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genders = await _serviceManager.PersonService.GetAllGendersAsync(); // Fetch genders from database
            return View(genders);  // Pass the genders to the view
        }
    }
}
