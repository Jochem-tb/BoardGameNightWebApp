using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Entities
{
    public class FoodOptions
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public required string Name { get; set; }
        public ICollection<Person> Persons { get; set; } = new List<Person>();
        public ICollection<GameNight> GameNights { get; set; } = new List<GameNight>();
    }
}
