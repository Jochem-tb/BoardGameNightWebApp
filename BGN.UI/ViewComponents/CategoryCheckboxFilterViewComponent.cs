using BGN.Services.Abstractions.FilterModels;
using BGN.Services.Abstractions;
using BGN.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.ViewComponents
{
    public class CategoryCheckboxFilterViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;

        public CategoryCheckboxFilterViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(AbstractGameFilterObject filterObject)
        {

            var categories = await _serviceManager.GameService.GetAllCategoriesAsync(); // Fetch categories from database

            filterObject.AvailableCategories = categories ?? new List<CategoryDto>(); // Ensure it's not null
            filterObject.SelectedCategories = filterObject.SelectedCategories ?? new List<int>(); // Ensure it's not null

            return View("~/Views/Shared/Components/Category/CategoryCheckboxFilterViewComponent.cshtml", filterObject);  // Pass the genres to the view
        }
    }
}
