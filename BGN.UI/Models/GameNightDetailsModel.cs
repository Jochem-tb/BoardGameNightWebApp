using BGN.Shared;

namespace BGN.UI.Models
{
    public class GameNightDetailsModel
    {
        public required GameNightDto GameNight { get; set; } 
        public required PersonDto CurrentUser { get; set; }

    }
}
