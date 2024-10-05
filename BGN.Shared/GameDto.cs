using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Shared
{
    public class GameDto
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        public int? MinPlayers { get; set; }

        public int? MaxPlayers { get; set; }

        public bool IsAdult { get; set; } = false;

        public required GenreDto Genre { get; set; }

        public int? GenreId { get; set; }

        public required CategoryDto Category { get; set; }

        public int? CategoryId { get; set; }
        public int? EstimatedTime { get; set; }
        public required string ImgUrl { get; set; }
    }
}
