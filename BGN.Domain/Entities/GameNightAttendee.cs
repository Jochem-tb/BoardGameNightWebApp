using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Entities
{
    public class GameNightAttendee
    {
        //Properties for the many-to-many relationship between GameNight and Person
        public int GameNightId { get; set; }
        
        public int AttendeeId { get; set; }

        //Navigation properties
        public Person Attendee { get; set; } = null!;
        public GameNight GameNight { get; set; } = null!;

        //Property for the attendance status
        public bool? AttendanceStatus { get; set; } = null;
    }
}
