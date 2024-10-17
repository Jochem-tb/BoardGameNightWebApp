using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<GameNight> GetByIdAsync(int id)
        {
            return await Task.FromResult(_dbContext.GameNights
                .Include(x => x.Games)
                .Include(x => x.Attendees)
                .Include(x => x.FoodOptions)
                .Include(x => x.Organiser)
                .FirstOrDefault(x => x.Id == id)
                );
        }


        public async Task<IQueryable<GameNight>> GetAllGameNightsAsQueryableAsync()
        {
            return await Task.FromResult(_dbContext.GameNights.AsQueryable()
                .Include(x => x.Games)
                .Include(x => x.Attendees)
                .Include(x => x.FoodOptions)
                .Include(x => x.Organiser));
        }

        public async Task<bool> JoinGameNightAsync(int gameNightId, string identityUserKey)
        {
            var gameNight = await _dbContext.GameNights
                .Include(x => x.Attendees)
                .FirstOrDefaultAsync(x => x.Id == gameNightId);

            var person = await _dbContext.Persons.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserKey);

            if (gameNight == null || person == null)
            {
                //Game night or person not found
                return false;
            }
            else
            {
                //Add the person to the game night
                gameNight.Attendees.Add(person);

                //Save changes
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> LeaveGameNightAsync(int gameNightId, string identityUserKey)
        {
            var gameNight = await _dbContext.GameNights
                .Include(x => x.Attendees)
                .FirstOrDefaultAsync(x => x.Id == gameNightId);

            var person = await _dbContext.Persons.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserKey);

            if (gameNight == null || person == null)
            {
                //Game night or person not found
                return false;
            }
            else
            {
                //Remove the person from the game night
                gameNight.Attendees.Remove(person);

                //Save changes
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        public Task<IEnumerable<Person>> GetAttendeesAsync(int gameNightId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Game>> GetGamesAsync(int gameNightId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FoodOptions>> GetFoodOptionsAsync(int gameNightId)
        {
            throw new NotImplementedException();
        }


        public async void Insert(GameNight gameNight)
        {
            _dbContext.Games.AttachRange(gameNight.Games);
            _dbContext.GameNights.Add(gameNight);
            _dbContext.SaveChanges();
        }

        public void Remove(GameNight gameNight)
        {
            _dbContext.GameNights.Remove(gameNight);
            _dbContext.SaveChanges();
        }
        public void Update(GameNight gameNight)
        {
            _dbContext.Update(gameNight);
            _dbContext.SaveChanges();
        }
        private void LogTrackedEntities()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                Debug.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}, Key: {entry.Property("Id").CurrentValue}");
            }
        }
    }
}
