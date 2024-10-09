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

        public Task<GameNightDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GameNightDto> UpdateAsync(int id, GameNightDto gameNight)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GameNightDto>> GetAllAsync(AbstractGameNightFilterObject filterObject)
        {
            var gameNightQuery = await _repositoryManager.GameNightRepository.GetAllGameNightsAsQueryableAsync();


            //Search by isAdult

            if (filterObject.SearchIsAdult.HasValue)
            {
                gameNightQuery = filterObject.SearchIsAdult.Value
                    ? gameNightQuery.Where(x => x.Games.Any(g => g.IsAdult) == true)

                    : gameNightQuery.Where(x => x.Games.Any(g => g.IsAdult) == false);
            }

            //Search by organizer name
            if (filterObject.SearchOrganizerName != null)
            {
                gameNightQuery = gameNightQuery.Where(x => (x.Organiser.FirstName + " " + x.Organiser.LastName)
                .Contains(filterObject.SearchOrganizerName));
            }

            //Search by game name
            if (filterObject.SearchGameName != null)
            {
                gameNightQuery = gameNightQuery.Where(x => x.Games.Any(y => y.Name.Contains(filterObject.SearchGameName)));
            }

            //Search by food options 
            if (filterObject.SelectedFoodOptions!.Any())
            {
                gameNightQuery = gameNightQuery.Where(x => x.FoodOptions != null && filterObject.SelectedFoodOptions!.Contains(x.Id));

            }

            var gameNightList = gameNightQuery.ToList();
            return _mapper.Map<List<GameNightDto>>(gameNightList);
        }
    }
}
