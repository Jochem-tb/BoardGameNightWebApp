using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Shared
{
    public class GameNightAttendeeDto
    {
        public required PersonDto Attendee { get; set; }
        public required GameNightDto GameNight { get; set; }
        public bool? AttendanceStatus { get; set; } = null;
    }
}
