using AutoMapper;
using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services
{
    internal class GameService : IGameService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public GameService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GameDto>> GetAllAsync()
        {
            var games = await _repositoryManager.GameRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repositoryManager.GameRepository.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<IEnumerable<GameDto>> GetAllGameByCategoryIdAsync(int catId)
        {
            var games = await _repositoryManager.GameRepository.GetAllGameByCategoryIdAsync(catId);
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }

        public async Task<IEnumerable<GameDto>> GetAllGameByGenreIdAsync(int genId)
        {
            var games = await _repositoryManager.GameRepository.GetAllGameByGenreIdAsync(genId);
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }

        public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
        {
            var genres = await _repositoryManager.GameRepository.GetAllGenresAsync();
            return _mapper.Map<IEnumerable<GenreDto>>(genres);
        }

        public async Task<GameDto> GetByIdAsync(int id)
        {
            var game = await _repositoryManager.GameRepository.GetByIdAsync(id);
            return _mapper.Map<GameDto>(game);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _repositoryManager.GameRepository.GetCategoryByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<GameDto>> GetEighteenPlusAsync(bool isAdult)
        {
            var games = await _repositoryManager.GameRepository.GetEighteenPlusAsync(isAdult);
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }

        public async Task<GenreDto> GetGenreByIdAsync(int id)
        {
            var genre = await _repositoryManager.GameRepository.GetGenreByIdAsync(id);
            return _mapper.Map<GenreDto>(genre);
        }

        public void Insert(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);
            _repositoryManager.GameRepository.Insert(game);
        }

        public void Remove(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);
            _repositoryManager.GameRepository.Remove(game);
        }
    }
}
