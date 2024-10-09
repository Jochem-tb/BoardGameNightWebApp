﻿using System;
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
        public required string OrganiserFirstname { get; set; }
        public string? OrganiserLastname { get; set; }
        public required string Street { get; set; }
        public required string HouseNr { get; set; }
        public required string City { get; set; }
        public int MaxPlayers { get; set; }

        public IEnumerable<GameDto> Games { get; set; } = new List<GameDto>();
    public IEnumerable<PersonDto> Attendees { get; set; } = new List<PersonDto>();
        //public IEnumerable<FoodOptionsDto> Foods { get; set; }
    }
}
