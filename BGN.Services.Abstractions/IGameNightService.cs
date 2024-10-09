using BGN.Services.Abstractions.FilterModels;
using BGN.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services.Abstractions
{
    public interface IGameNightService
    {
        Task<IEnumerable<GameNightDto>> GetAllAsync();
        Task<IEnumerable<GameNightDto>> GetAllAsync(AbstractGameNightFilterObject filterObject);
        Task<GameNightDto> GetByIdAsync(int id);
        Task<GameNightDto> CreateAsync(GameNightDto gameNight);
        Task<GameNightDto> UpdateAsync(int id, GameNightDto gameNight);
        Task DeleteAsync(int id);
    }
}
