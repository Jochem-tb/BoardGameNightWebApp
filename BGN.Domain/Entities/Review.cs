using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Details { get; set; }

        [Required]
        [Range(1, 5)]
        public required int Rating { get; set; }

        [Required]
        public required Person Reviewer { get; set; }

        public int? ReviewerId { get; set; }

        [Required]
        public required GameNight GameNight { get; set; }

        public int? GameNightId { get; set; }

    }
}
