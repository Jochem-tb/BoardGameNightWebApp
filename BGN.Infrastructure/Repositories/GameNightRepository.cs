using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Infrastructure.Repositories
{
    internal sealed class GameNightRepository : IGameNightRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public GameNightRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<GameNight>> GetAllAsync()
        {
            return await _dbContext.GameNights.ToListAsync();
        }

        public Task<GameNight> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(GameNight gameNight)
        {
            throw new NotImplementedException();
        }

        public void Remove(GameNight gameNight)
        {
            throw new NotImplementedException();
        }
    }
}
