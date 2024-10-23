using BGN.Domain.Entities;
using BGN.Services.Abstractions.FilterModels;
using BGN.Shared;

namespace BGN.UI.Models
{
    public class GameNightListModel : AbstractGameNightFilterObject
    {
        //Filter terms for GameNight from Abstract Class

        public IEnumerable<GameNightDto> DisplayGameNights { get; set; } = new List<GameNightDto>();
        public required PersonDto CurrentUser { get; set; }
    }
}
