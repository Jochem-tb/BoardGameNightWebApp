using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameNight.Domain
{
    internal class GameNight
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public required DateTime Date { get; set; }
        public required TimeSpan Time { get; set; }
        public required Person Organiser { get; set; }
        public required string Street { get; set; }
        public required string HouseNr { get; set; }
        public required string City { get; set; }
        public required int MaxPlayers { get; set; }

        public ICollection<Person> Attendees { get; set; } = new List<Person>();
        public ICollection<Game> Games { get; set; } = new List<Game>();

        public ICollection<Review> reviews { get; set; } = new List<Review>();


    }
}
