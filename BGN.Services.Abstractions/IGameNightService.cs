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

        Task<bool> JoinGameNightAsync(int gameNightId, string identityUserKey);
        Task<bool> LeaveGameNightAsync(int gameNightId, string identityUserKey);

        Task<IEnumerable<PersonDto>> GetAttendeesAsync(int gameNightId);
        Task<IEnumerable<GameDto>> GetGamesAsync(int gameNightId);
        Task<IEnumerable<FoodOptionDto>> GetFoodOptionsAsync(int gameNightId);

    }
}
