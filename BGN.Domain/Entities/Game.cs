using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public required string Name { get; set; }

        
        [MaxLength(255)]
        public string? Description { get; set; }


        [Range(1, 100)]
        public int? MinPlayers { get; set; }


        [Range(1, 255)]
        public int? MaxPlayers { get; set; }


        public bool IsAdult { get; set; } = false;

        [Required]
        public required Genre Genre { get; set; }

        public int? GenreId { get; set; }

        [Required]
        public required Category Category { get; set; }

        public int? CategoryId { get; set; }

        [Range(1, 720)] // Maximum playtime = 12 hours
        public int? EstimatedTime { get; set; }

        //TODO : Add a property for the image

        public ICollection<GameNight> GameNights { get; set; } = new List<GameNight>();
    }
}
