using BGN.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.ViewComponents
{
    public class CategoryDropdownViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;
        public CategoryDropdownViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _serviceManager.GameService.GetAllCategoriesAsync(); // Fetch categories from database
            return View("~/Views/Shared/Components/Category/CategoryDropdown.cshtml", categories);  // Pass the categories to the view
        }
    }
    

    
}
