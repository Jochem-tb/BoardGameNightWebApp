﻿using AutoMapper;
using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.Services.Abstractions.FilterModels;
using BGN.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services
{
    internal class GameService(IRepositoryManager repositoryManager, IMapper mapper) : IGameService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

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

        public async Task<IEnumerable<GameDto>> GetAllAsync(AbstractGameFilterObject filterObject)
        {
            // Get the base query with all games
            var gamesQueryable = await _repositoryManager.GameRepository.GetAllGamesAsQueryableAsync();

            // Apply filters to the query
            if (!string.IsNullOrEmpty(filterObject.SearchName))
            {
                gamesQueryable = gamesQueryable.Where(x => x.Name.Contains(filterObject.SearchName));
            }

            // Apply age restriction filter
            if (filterObject.SearchIsAdult.HasValue)
            {
                gamesQueryable = filterObject.SearchIsAdult.Value
                    ? gamesQueryable.Where(x => x.IsAdult == true)
                    : gamesQueryable.Where(x => x.IsAdult == false);
            }
            // "All" case: No filtering on IsAdult
            // No need to apply a filter in this case.


            // Apply player count filter
            if (filterObject.SearchMinPlayers.HasValue)
            {
                gamesQueryable = gamesQueryable.Where(x => x.MinPlayers >= filterObject.SearchMinPlayers.Value);
            }

            // Apply maximum players filter; if Unlimited is selected, skip max player filtering
            if (filterObject.SearchMaxPlayers.HasValue)
            {
                gamesQueryable = gamesQueryable.Where(x => x.MaxPlayers <= filterObject.SearchMaxPlayers.Value);
            }

            if (filterObject.SelectedGenres!.Any())
            {
                gamesQueryable = gamesQueryable.Where(g =>  filterObject.SelectedGenres!.Contains(g.GenreId));
            }

            if (filterObject.SelectedCategories!.Any())
            {
                gamesQueryable = gamesQueryable.Where(g =>  filterObject.SelectedCategories!.Contains(g.CategoryId));
            }

            if (filterObject.SearchEstimatedTimeLower.HasValue)
            {
                gamesQueryable = gamesQueryable.Where(x => x.EstimatedTime >= filterObject.SearchEstimatedTimeLower);
            }

            if (filterObject.SearchEstimatedTimeUpper.HasValue)
            {
                gamesQueryable = gamesQueryable.Where(x => x.EstimatedTime <= filterObject.SearchEstimatedTimeUpper);
            }

            // Materialize the query and map to DTOs
            var gamesList = gamesQueryable.ToList(); //Calls the database when the Query is done
            return _mapper.Map<IEnumerable<GameDto>>(gamesList);
        }

        public async Task<IEnumerable<GenreDto>> GetAllGenresAsync()
        {
            var genres = await _repositoryManager.GameRepository.GetAllGenresAsync();
            return _mapper.Map<IEnumerable<GenreDto>>(genres);
        }

        public async Task<GameDto?> GetByIdAsync(int id)
        {
            var game = await _repositoryManager.GameRepository.GetByIdAsync(id);
            return _mapper.Map<GameDto>(game);
        }

        public void Insert(Game game)
        {
            _repositoryManager.GameRepository.Insert(game);
        }

        public void Remove(Game game)
        {
            _repositoryManager.GameRepository.Remove(game);
        }

        public async Task UpdateAsync(Game game)
        {
            var existingGame = await _repositoryManager.GameRepository.GetByIdAsync(game.Id);

            if (existingGame != null)
            {
                existingGame.Name = game.Name;
                existingGame.Description = game.Description;
                existingGame.MinPlayers = game.MinPlayers;
                existingGame.MaxPlayers = game.MaxPlayers;
                existingGame.IsAdult = game.IsAdult;
                existingGame.GenreId = game.GenreId;
                existingGame.Genre = game.Genre;
                existingGame.CategoryId = game.CategoryId;
                existingGame.Category = game.Category;
                existingGame.EstimatedTime = game.EstimatedTime;
                existingGame.ImgUrl = game.ImgUrl;
                
                _repositoryManager.GameRepository.Update(existingGame);
            }
        }

        public async Task<IEnumerable<Game>> GetAllEntityAsync()
        {
            return await _repositoryManager.GameRepository.GetAllAsync();
        }
    }
}
