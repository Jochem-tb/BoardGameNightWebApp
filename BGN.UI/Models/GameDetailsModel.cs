using BGN.Shared;

namespace BGN.UI.Models
{
    public class GameDetailsModel
    {
        public required GameDto Game{ get; set; }
        public required IEnumerable<GameNightDto> GameNightsWithThisGame { get; set; }
        public required PersonDto? CurrentUser { get; set; }

    }
}
