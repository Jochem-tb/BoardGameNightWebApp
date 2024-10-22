using BGN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Repositories
{
    public interface IGameNightRepository
    {
        Task<IEnumerable<GameNight>> GetAllAsync();
        Task<IQueryable<GameNight>> GetAllGameNightsAsQueryableAsync();
        Task<GameNight> GetByIdAsync(int id);
        void Insert(GameNight gameNight);
        void Update(GameNight gameNight);
        void Remove(GameNight gameNight);
        void UpdateAttendance(GameNight gameNight);

        Task<bool> JoinGameNightAsync(int gameNightId, string identityUserKey);
        Task<bool> LeaveGameNightAsync(int gameNightId, string identityUserKey);

        Task<IEnumerable<Person>> GetAttendeesAsync(int gameNightId);
        Task<IEnumerable<Game>> GetGamesAsync(int gameNightId);
        Task<IEnumerable<FoodOptions>> GetFoodOptionsAsync(int gameNightId);
    }
}
