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
            return await _dbContext.Games.ToListAsync();
        }


        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }


        public async Task<IEnumerable<Game>> GetAllGameByCategoryIdAsync(int catId)
        {
            return await _dbContext.Games.Where(x => x.CategoryId == catId)
                .Include(x => x.Category)
                .Include(x => x.Genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetAllGameByGenreIdAsync(int genId)
        {
            return await _dbContext.Games.Where(x => x.GenreId == genId)
                .ToListAsync();
        }

        public async Task<IQueryable<Game>> GetAllGamesAsQueryableAsync()
        {
            return await Task.FromResult(_dbContext.Games.AsQueryable()
                .Include(x => x.Category)
                .Include(x => x.Genre)
                );
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _dbContext.Genres.ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _dbContext.Games.Where(x => x.Id == id)
                .Include(x => x.Category)
                .Include(x => x.Genre)
                .FirstOrDefaultAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Categories.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Game>> GetEighteenPlusAsync(bool isAdult)
        {
            return await _dbContext.Games.Where(x => x.IsAdult == isAdult)
                .ToListAsync();
        }

        public async Task<Genre?> GetGenreByIdAsync(int id)
        {
            return await _dbContext.Genres.Where(x => x.Id == id)
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
    }
}
