using BGN.Domain.Entities;
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
        Task<IEnumerable<GameNightDto>> GetAllWithGameIdAsync(int gameId);

        Task<GameNightDto> GetByIdAsync(int id);
        Task<GameNight?> GetByIdEntityAsync(int id);

        void Insert(GameNight gameNight);
        Task UpdateAsync(GameNight gameNight);
        Task<bool> DeleteByIdAsync(int gameNightId, string identityUserKey);

        void UpdateAttendance(GameNight gameNight);
        Task<bool?> JoinGameNightAsync(int gameNightId, string identityUserKey);
        Task<bool> LeaveGameNightAsync(int gameNightId, string identityUserKey);

    }
}
