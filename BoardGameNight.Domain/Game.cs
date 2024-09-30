using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameNight.Domain
{
    internal class Game
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? MinPlayers { get; set; }
        public int? MaxPlayers { get; set; }
        public bool IsAdult { get; set; } = false;
        
        public Genre genre { get; set; }
        public int? GenreId { get; set; }

        public Category category { get; set; }
        public int? CategoryId { get; set; }
        public int? EstimatedTime { get; set; }

        //TODO : Add a property for the image
    }
}
