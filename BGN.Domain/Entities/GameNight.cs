using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Entities
{
    public class GameNight
    {

        public int Id { get; set; }


        [MaxLength(50)] // You can name your GameNight.
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public required DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public required TimeSpan Time { get; set; }

        [ValidateNever]
        public required Person Organiser { get; set; }

        [Required]
        public required int OrganiserId { get; set; }

        [Required]
        [MaxLength(80)]
        public required string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public required string HouseNr { get; set; }

        [Required]
        [MaxLength(50)]
        public required string City { get; set; }

        [Required]
        [Range(1, 255)] // You can have a maximum of 255 players. --> Trivia games have > 100 players.
        public required int MaxPlayers { get; set; }

        public bool OnlyAdultWelcome { get; set; } = false;

        [Required]
        [MaxLength(255)]
        public required string ImgUrl { get; set; } = "/img/gamenight/BoardGameDefaultSmall.jpg";

        public ICollection<GameNightAttendee> Attendees { get; set; } = new List<GameNightAttendee>();
        public ICollection<Game> Games { get; set; } = new List<Game>();


        public ICollection<FoodOptions> FoodOptions { get; set; } = new List<FoodOptions>();


    }
}
