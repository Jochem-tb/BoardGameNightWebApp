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
        Task<GameNight> GetByIdAsync(int id);
        void Insert(GameNight gameNight);
        void Remove(GameNight gameNight);
    }
}
