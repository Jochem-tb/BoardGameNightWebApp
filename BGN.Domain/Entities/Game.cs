using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [Required]
        [Range(1, 100)]
        public required int MinPlayers { get; set; }

        [Required]
        [Range(1, 255)]
        public required int MaxPlayers { get; set; }


        public bool IsAdult { get; set; } = false;

        [ValidateNever]
        public required Genre Genre { get; set; }

        [Required]
        public required int GenreId { get; set; }

        [ValidateNever]
        public required Category Category { get; set; }

        [Required]
        public required int CategoryId { get; set; }

        [Required]
        [Range(1, 720)] // Maximum playtime = 12 hours
        public int? EstimatedTime { get; set; }

        [Required]
        [MaxLength(255)]
        public required string ImgUrl { get; set; } = "/img/gamenight/BoardGameDefaultSmall.jpg";

        public ICollection<GameNight> GameNights { get; set; } = new List<GameNight>();
    }
}
