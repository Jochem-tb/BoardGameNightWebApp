using BGN.Services.Abstractions;
using BGN.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.ViewComponents
{
    public class FoodOptionsCheckboxFilterViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;
        public FoodOptionsCheckboxFilterViewComponent(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(GameNightListModel gameNightListModel)
        {
            var foodOptions = await _serviceManager.MiscService.GetAllFoodOptionsAsync(); // Fetch foodoptions from database

            gameNightListModel.AvailableFoodOptions = foodOptions; // Ensure it's not null
            gameNightListModel.SelectedFoodOptions = gameNightListModel.SelectedFoodOptions ?? new List<int>(); // Ensure it's not null

            return View("~/Views/Shared/Components/FoodOption/FoodOptionsCheckboxFilter.cshtml", gameNightListModel);  // Pass the foodoptions to the view
        }

    }
}
