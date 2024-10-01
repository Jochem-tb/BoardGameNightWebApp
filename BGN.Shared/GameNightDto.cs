using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Shared
{
    public class GameNightDto
    {
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public PersonDto Organiser { get; set; }
        public string Street { get; set; }
        public string HouseNr { get; set; }
        public string City { get; set; }
        public int MaxPlayers { get; set; }
    }
}
