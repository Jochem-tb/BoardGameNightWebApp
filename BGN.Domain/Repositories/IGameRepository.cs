using BGN.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Domain.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);
        void Insert(Game game);
        void Remove(Game game);
        void Update(Game game);


        Task<IEnumerable<Game>> GetAllGameByGenreIdAsync(int genId);
        Task<IEnumerable<Game>> GetAllGameByCategoryIdAsync(int catId);

        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<IQueryable<Game>> GetAllGamesAsQueryableAsync();
    }
}
