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
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetAllAsync();
        Task<IEnumerable<Game>> GetAllEntityAsync();
        Task<GameDto?> GetByIdAsync(int id);
        void Insert(Game game);
        void Remove(Game game);
        Task UpdateAsync(Game game);

        Task<IEnumerable<GameDto>> GetAllGameByGenreIdAsync(int genId);
        Task<IEnumerable<GameDto>> GetAllGameByCategoryIdAsync(int catId);

        Task<IEnumerable<GenreDto>> GetAllGenresAsync();
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();

        Task<IEnumerable<GameDto>> GetAllAsync(AbstractGameFilterObject filterObject);
    }
}
