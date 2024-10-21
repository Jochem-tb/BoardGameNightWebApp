﻿using AutoMapper;
using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.Services.Abstractions.FilterModels;
using BGN.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services
{
    internal class GameNightService : IGameNightService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public GameNightService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public Task<GameNightDto> CreateAsync(GameNightDto gameNight)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GameNightDto>> GetAllAsync()
        {
            var gameNightList = await _repositoryManager.GameNightRepository.GetAllAsync();
            return _mapper.Map<List<GameNightDto>>(gameNightList);
        }

        public async Task<GameNightDto> GetByIdAsync(int id)
        {
            var gameNight = await _repositoryManager.GameNightRepository.GetByIdAsync(id);
            return _mapper.Map<GameNightDto>(gameNight);
        }

        public Task<GameNightDto> UpdateAsync(int id, GameNightDto gameNight)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GameNightDto>> GetAllAsync(AbstractGameNightFilterObject filterObject)
        {
            // Await the asynchronous call to get the IQueryable
            var gameNightQuery = await _repositoryManager.GameNightRepository.GetAllGameNightsAsQueryableAsync();

            // Search by isAdult
            if (filterObject.SearchIsAdult.HasValue)
            {
                // Use the correct filtering logic based on the isAdult search value
                gameNightQuery = filterObject.SearchIsAdult.Value
                    ? gameNightQuery.Where(x => x.Games.Any(g => g.IsAdult))
                    : gameNightQuery.Where(x => !x.Games.Any(g => g.IsAdult));
            }

            // Search by organizer name
            if (!string.IsNullOrEmpty(filterObject.SearchOrganizerName))
            {
                gameNightQuery = gameNightQuery.Where(x =>
                    (x.Organiser.FirstName + " " + x.Organiser.LastName).Contains(filterObject.SearchOrganizerName));
            }

            // Search by game name
            if (!string.IsNullOrEmpty(filterObject.SearchGameName))
            {
                gameNightQuery = gameNightQuery.Where(x => x.Games.Any(y => y.Name.Contains(filterObject.SearchGameName)));
            }

            // Check if any food options were selected
            if (filterObject.SelectedFoodOptions != null && filterObject.SelectedFoodOptions.Any())
            {
                //Gamenight must have ALL the selected options:
                gameNightQuery = gameNightQuery.Where(gameNight =>
                    filterObject.SelectedFoodOptions.All(selectedFoodOptionId =>
                        gameNight.FoodOptions.Any(fo => fo.Id == selectedFoodOptionId)));
            }

            // Execute the query and convert it to a list
            var gameNightList = gameNightQuery.ToList();

            // Map the results to GameNightDto
            return _mapper.Map<List<GameNightDto>>(gameNightList);
        }

        public async Task<bool?> JoinGameNightAsync(int gameNightId, string identityUserKey)
        {

            var gameNight = await _repositoryManager.GameNightRepository.GetByIdAsync(gameNightId);

            //Check if Attending == MaxPlayers
            //var isPlaceAvailable = gameNight.Where(x => x.Attendees.Count() < x.MaxPlayers).Any();
            var isPlaceAvailable = gameNight.Attendees.Count() < gameNight.MaxPlayers;
            if (!isPlaceAvailable)
            {
                return false;
            }

            //Check if person is already attending
            //var isAttending = gameNight.Where(x => x.Attendees.Any(y => y.IdentityUserId == identityUserKey)).Any();
            var isAttending = gameNight.Attendees.Any(y => y.IdentityUserId == identityUserKey);
            if (isAttending)
            {
                return false;
            }

            //Organiser can join his own game, only once as per check above

            //Check if user already has a game night on the same date
            var isAlreadyAttendingAnotherGameNightSameDay = _repositoryManager.GameNightRepository.GetAllGameNightsAsQueryableAsync().Result
                .Where(x => x.Attendees.Any(y => y.IdentityUserId == identityUserKey))
                .Any(x => x.Date == gameNight.Date);
            if(isAlreadyAttendingAnotherGameNightSameDay)
            {
                return null;
            }

            try
            {
                var isSuccess = await _repositoryManager.GameNightRepository.JoinGameNightAsync(gameNightId, identityUserKey);
                return isSuccess;
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Debug.WriteLine($"Error joining Game Night: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LeaveGameNightAsync(int gameNightId, string identityUserKey)
        {
            var gameNight = await _repositoryManager.GameNightRepository.GetByIdAsync(gameNightId);

            //Check if person is attending
            var isAttending = gameNight.Attendees.Any(y => y.IdentityUserId == identityUserKey);
            if (!isAttending)
            {
                return false;
            }
            gameNight = null;

            //Organiser can leave his own game
            var isSuccess = await _repositoryManager.GameNightRepository.LeaveGameNightAsync(gameNightId, identityUserKey);
            return isSuccess;
        }

        public Task<IEnumerable<PersonDto>> GetAttendeesAsync(int gameNightId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GameDto>> GetGamesAsync(int gameNightId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FoodOptionDto>> GetFoodOptionsAsync(int gameNightId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GameNightDto>> GetAllWithGameIdAsync(int gameId)
        {
            var query = await _repositoryManager.GameNightRepository.GetAllGameNightsAsQueryableAsync();
            query = query.Where(x => x.Games.Any(y => y.Id == gameId));

            var gameNightList = query.ToList();
            return _mapper.Map<IEnumerable<GameNightDto>>(gameNightList);

        }

        public void Insert(GameNight gameNight)
        {
            _repositoryManager.GameNightRepository.Insert(gameNight);
        }

        public async Task UpdateAsync(GameNightDto gameNightDto)
        {
            var gameNight = _mapper.Map<GameNight>(gameNightDto);
            await UpdateAsync(gameNight);
        }

        public async Task UpdateAsync(GameNight gameNight)
        {
            var existingGameNight = await _repositoryManager.GameNightRepository.GetByIdAsync(gameNight.Id);
            if (existingGameNight != null)
            {
                //Non-List information
                existingGameNight.Name = gameNight.Name;
                existingGameNight.Date = gameNight.Date;
                existingGameNight.Time = gameNight.Time;
                existingGameNight.Organiser.Id = gameNight.OrganiserId;
                existingGameNight.Street = gameNight.Street;
                existingGameNight.HouseNr = gameNight.HouseNr;
                existingGameNight.City = gameNight.City;
                existingGameNight.MaxPlayers = gameNight.MaxPlayers;
                existingGameNight.OnlyAdultWelcome = gameNight.OnlyAdultWelcome;
                existingGameNight.ImgUrl = gameNight.ImgUrl;

                //List information
                //existingGameNight.Attendees = gameNight.Attendees;
                existingGameNight.Games = gameNight.Games;
                existingGameNight.FoodOptions = gameNight.FoodOptions;

                _repositoryManager.GameNightRepository.Update(existingGameNight);
            }
        }


        public async Task DeleteByIdAsync(int id)
        {
            var gameNight = await _repositoryManager.GameNightRepository.GetByIdAsync(id);
            if (gameNight != null)
            {
                _repositoryManager.GameNightRepository.Remove(gameNight);
            }
        }

        public void Delete(GameNight gameNight)
        {
            _repositoryManager.GameNightRepository.Remove(gameNight);
        }

        public async Task<GameNight> GetByIdEntityAsync(int id)
        {
            return await _repositoryManager.GameNightRepository.GetByIdAsync(id);
        }
    }
}
