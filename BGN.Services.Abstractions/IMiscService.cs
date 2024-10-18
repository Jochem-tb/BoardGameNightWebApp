using BGN.Domain.Entities;
using BGN.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services.Abstractions
{
    public interface IMiscService
    {
        public Task<IEnumerable<FoodOptionDto>> GetAllFoodOptionsAsync();
        public Task<IEnumerable<FoodOptions>> GetAllFoodOptionsEntityAsync();
        public Task<IEnumerable<FoodOptionDto>> GetAllFoodOptionDtoByIdAsync(int[] array);
    }
}
