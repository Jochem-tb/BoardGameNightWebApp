using BGN.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        // New properties for genres and categories
        public IEnumerable<int>? SelectedGenres { get; set; } 
        public IEnumerable<int>? SelectedCategories { get; set; } 

        // Available genres and categories from the database
        public IEnumerable<GenreDto> AvailableGenres { get; set; } = new List<GenreDto>();
        public IEnumerable<CategoryDto> AvailableCategories { get; set; } = new List<CategoryDto>();
    }
}
