using BGN.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.ViewComponents
{
    public class GenreDropdownViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;
        public GenreDropdownViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await _serviceManager.GameService.GetAllGenresAsync(); // Fetch genres from database
            return View("~/Views/Shared/Components/Genre/GenreDropdown.cshtml", genres);  // Pass the categories to the view
        }
    }
    

    
}
