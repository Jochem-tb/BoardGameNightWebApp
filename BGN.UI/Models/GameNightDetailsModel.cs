using BGN.Shared;

namespace BGN.UI.Models
{
    public class GameNightDetailsModel
    {
        public required GameNightDto GameNight { get; set; } 
        public required PersonDto CurrentUser { get; set; }

        public Dictionary<int, float> ShowNoShowDictionary { get; set; } = new();

        public GameListModel GameListModel;
        public GameListModel GetGameListModel()
        {
            return new()
            {
                DisplayGames = GameNight.Games,
                CurrentUser = this.CurrentUser
            };
        }

    }
}
