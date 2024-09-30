﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameNight.Domain
{
    public class Review
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Details { get; set; }
        public required int Rating { get; set; }
        public required Person Reviewer { get; set; }
        public required int ReviewerId { get; set; }
        public required Game Game { get; set; }
        public required int GameId { get; set; }

    }
}
