using BGN.Shared;
using BGN.Services.Abstractions.FilterModels;

namespace BGN.UI.Models
{
    public class GameListModel : AbstractGameFilterObject
    {
        //Filter terms for Game
        //Already defined in the AbstractGameFilterObject class..

        //List with games to display
        public IEnumerable<GameDto> DisplayGames { get; set; } = new List<GameDto>();

    }
}
