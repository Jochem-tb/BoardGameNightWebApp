using BGN.Domain.Entities;
using BGN.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BGN.UI.Models
{
    public class CrudGameNightModel : CrudModel
    {
        public GameNight? GameNight { get; set; }
        public IFormFile? CoverPhoto { get; set; }
        public GameListModel? GameListModel { get; set; }
        public int[] SelectedGameIds { get; set; } = new int[] { };
        public int[] SelectedFoodOptionIds { get; set; } = new int[] { };
        public IEnumerable<FoodOptionDto> FoodOptions { get; set; } = new List<FoodOptionDto>();

    }
}
