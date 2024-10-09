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
            return await Task.FromResult(_dbContext.GameNights
                .Include(x => x.Games)
                .Include(x => x.Attendees)
                .Include(x => x.FoodOptions)
                .Include(x => x.Organiser));
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

        public async Task<IQueryable<GameNight>> GetAllGameNightsAsQueryableAsync()
        {
            return await Task.FromResult(_dbContext.GameNights.AsQueryable()
                .Include(x => x.Games)
                .Include(x => x.Attendees)
                .Include(x => x.FoodOptions)
                .Include(x => x.Organiser));
        }
    }
}
