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
    internal sealed class GameRepository : IGameRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public GameRepository(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _dbContext.Games.AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<Game>> GetAllGameByCategoryIdAsync(int catId)
        {
            return await _dbContext.Games.Where(x => x.CategoryId == catId)
                .Include(x => x.Category)
                .Include(x => x.Genre)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetAllGameByGenreIdAsync(int genId)
        {
            return await _dbContext.Games.AsNoTracking().Where(x => x.GenreId == genId)
                .ToListAsync();
        }

        public async Task<IQueryable<Game>> GetAllGamesAsQueryableAsync()
        {
            return await Task.FromResult(_dbContext.Games.AsNoTracking().AsQueryable()
                .Include(x => x.Category).AsNoTracking()
                .Include(x => x.Genre).AsNoTracking()
                );
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _dbContext.Genres.AsNoTracking().ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _dbContext.Games.AsNoTracking().Where(x => x.Id == id)
                .Include(x => x.Category)
                .Include(x => x.Genre)
                .FirstOrDefaultAsync();
        }

        public void Insert(Game game)
        {
            _dbContext.Games.Add(game);
            _dbContext.SaveChanges();
        }

        public void Remove(Game game)
        {
            _dbContext.Games.Remove(game);
            _dbContext.SaveChanges();
        }

        public void Update(Game game)
        {
            
            _dbContext.Games.Update(game);
            _dbContext.SaveChanges();

        }
    }
}
