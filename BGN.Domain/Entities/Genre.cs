using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public required string Name { get; set; }
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
