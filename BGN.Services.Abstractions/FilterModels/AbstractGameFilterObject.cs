using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services.Abstractions.FilterModels
{
    public abstract class AbstractGameFilterObject
    {
        public string? SearchName { get; set; }
        public int? SearchMinPlayers { get; set; }
        public int? SearchMaxPlayers { get; set; }
        public bool? SearchIsAdult { get; set; }
        public int? SearchGenreId { get; set; }
        public int? SearchCategoryId { get; set; }
        public int? SearchEstimatedTimeLower { get; set; }
        public int? SearchEstimatedTimeUpper { get; set; }
    }
}
