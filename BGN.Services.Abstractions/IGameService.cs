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
        void Insert(GameDto gameDto);
        void Insert(Game game);
        void Remove(GameDto gameDto);
        void Remove(Game game);
        Task UpdateAsync(GameDto gameDto);
        Task UpdateAsync(Game game);


        Task<IEnumerable<GameDto>> GetAllByIdAsync(int[] array);
        Task<IEnumerable<GameDto>> GetAllGameByGenreIdAsync(int genId);
        Task<IEnumerable<GameDto>> GetAllGameByCategoryIdAsync(int catId);
        Task<IEnumerable<GameDto>> GetEighteenPlusAsync(bool isAdult);

        Task<IEnumerable<GenreDto>> GetAllGenresAsync();
        Task<GenreDto?> GetGenreByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);

        Task<IEnumerable<GameDto>> GetAllAsync(AbstractGameFilterObject filterObject);
    }
}
