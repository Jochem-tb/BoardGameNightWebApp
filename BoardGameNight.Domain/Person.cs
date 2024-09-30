using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameNight.Domain
{
    internal class Person
    {

        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public Gender? Gender { get; set; }
        public int? GenderId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Street { get; set; }
        public required string HouseNr { get; set; }
        public required string PostalCode { get; set; }
        public string? City { get; set; }

    }
}
