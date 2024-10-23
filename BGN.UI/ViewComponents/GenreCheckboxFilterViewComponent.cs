using BGN.Services.Abstractions.FilterModels;
using BGN.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using BGN.Shared;

namespace BGN.UI.ViewComponents
{
    public class GenreCheckboxFilterViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;

        public GenreCheckboxFilterViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(AbstractGameFilterObject filterObject)
        {

            var genres = await _serviceManager.GameService.GetAllGenresAsync(); // Fetch genres from database

            filterObject.AvailableGenres = genres ?? new List<GenreDto>(); // Ensure it's not null
            filterObject.SelectedGenres = filterObject.SelectedGenres ?? new List<int>(); // Ensure it's not null
                                                                                          
            return View("~/Views/Shared/Components/Genre/GenreCheckboxFilterViewComponent.cshtml", filterObject);  // Pass the genres to the view
        }
    }
}
