using AutoMapper;
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

        public Task<GameNightDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GameNightDto> UpdateAsync(int id, GameNightDto gameNight)
        {
            throw new NotImplementedException();
        }
    }
}
