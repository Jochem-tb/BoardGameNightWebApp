using AutoMapper;
using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.Services.Abstractions.FilterModels;
using BGN.Shared;
using System;
using System.Collections.Generic;
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

            //TODO: Apply filters

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
                gameNightQuery = gameNightQuery.Where(gameNight =>
                    gameNight.FoodOptions.Any(fo => filterObject.SelectedFoodOptions.Contains(fo.Id)));
            }

            // Execute the query and convert it to a list
            var gameNightList = gameNightQuery.ToList(); 

            // Map the results to GameNightDto
            return _mapper.Map<List<GameNightDto>>(gameNightList);
        }

        public async Task<bool> JoinGameNightAsync(int gameNightId, string identityUserKey)
        {
            var gameNights = await _repositoryManager.GameNightRepository.GetAllAsync();
            gameNights = gameNights.Where(x => x.Id == gameNightId).ToList();

            //Check if Attending == MaxPlayers
            var isPlaceAvailable = gameNights.Where(x => x.Attendees.Count() < x.MaxPlayers).Any();
            if (!isPlaceAvailable)
            {
                return false;
            }

            //Check if person is already attending
            var isAttending = gameNights.Where(x => x.Attendees.Any(y => y.IdentityUserId == identityUserKey)).Any();
            if (isAttending)
            {
                return false;
            }

            //Check if person is the organizer
            var isOrganiser = gameNights.Where(x => x.Organiser.IdentityUserId == identityUserKey).Any();
            if (isOrganiser)
            {
                return false;
            }

            var isSuccess = await _repositoryManager.GameNightRepository.JoinGameNightAsync(gameNightId, identityUserKey);

            return isSuccess;
        }

        public Task<bool> LeaveGameNightAsync(int gameNightId, string identityUserKey)
        {
            throw new NotImplementedException();
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
    }
}
