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
    internal class MiscService : IMiscService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public MiscService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FoodOptionDto>> GetAllFoodOptionDtoByIdAsync(int[] array)
        {
            var foodOptions = await _repositoryManager.MiscRepository.GetAllFoodOptionsAsync();
            foodOptions = foodOptions.Where(x => array.Contains(x.Id)).ToList();
            return await Task.FromResult(_mapper.Map<IEnumerable<FoodOptionDto>>(foodOptions));

        }

        public async Task<IEnumerable<FoodOptionDto>> GetAllFoodOptionsAsync()
        {
            var foodOptions = await _repositoryManager.MiscRepository.GetAllFoodOptionsAsync();
            return await Task.FromResult(_mapper.Map<IEnumerable<FoodOptionDto>>(foodOptions));
        }

        public async Task<IEnumerable<FoodOptions>> GetAllFoodOptionsEntityAsync()
        {
            return await _repositoryManager.MiscRepository.GetAllFoodOptionsAsync();
        }
    }
}
