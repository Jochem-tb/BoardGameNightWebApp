using BGN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Shared
{
    public class PersonDto
    {
        public int Id { get; set; }
        public  string? FirstName { get; set; }
        public  string? LastName { get; set; }
        public  string? Email { get; set; }

        public required string IdentityUserId { get; init; }

        public GenderDto? Gender { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Street { get; set; }

        public required string HouseNr { get; set; }

        public required string PostalCode { get; set; }

        public required string City { get; set; }
        public required bool AttendanceStatus { get; set; } = false;

        public IEnumerable<FoodOptionDto> Preferences { get; set; } = new List<FoodOptionDto>();

        public bool IsAdult()
        {
            return DateOfBirth.AddYears(18) <= DateTime.Now;
        }
    }
}

