using BGN.Domain.Entities;
using BGN.Shared;

namespace BGN.Services.Abstractions.FilterModels
{
    public abstract class AbstractGameNightFilterObject
    {
        public string? SearchOrganizerName { get; set; }
        public bool? SearchIsAdult { get; set; }
        public string? SearchGameName { get; set; }


        public IEnumerable<int>? SelectedFoodOptions { get; set; } = new List<int>();
        public IEnumerable<FoodOptionDto> AvailableFoodOptions { get; set; } = new List<FoodOptionDto>();
    }
}