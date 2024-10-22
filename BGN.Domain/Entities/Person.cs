using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Entities
{
    public class Person
    {

        public int Id { get; set; }

        //FK to link with IdentityUser
        [MaxLength(450)] //Same Lenght as in AuthDb
        public required string IdentityUserId { get; init; }

        [Required]
        [MaxLength(30)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public required string LastName { get; set; }

        [Required]
        [MaxLength(60)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public Gender? Gender { get; set; }

        public int GenderId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Phone]
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(80)]
        public required string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public required string HouseNr { get; set; }

        [Required]
        [MaxLength(10)]
        public required string PostalCode { get; set; }

        [Required]
        [MaxLength(50)]
        public required string City { get; set; }

        public ICollection<GameNightAttendee> GameNights { get; set; } = new List<GameNightAttendee>();

        public ICollection<FoodOptions> Preferences { get; set; } = new List<FoodOptions>();

    }
}
