﻿using AutoMapper;
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
    internal sealed class PersonService : IPersonService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public PersonService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public Task<PersonDto> CreateAsync(PersonDto person)
        {
            throw new NotImplementedException();
        }

        public void Insert(Person person)
        {
            _repositoryManager.PersonRepository.Insert(person);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PersonDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PersonDto> GetByIdAsync(int id)
        {
            var dbPerson = await _repositoryManager.PersonRepository.GetByIdAsync(id);
            return _mapper.Map<PersonDto>(dbPerson);

        }

        public Task<PersonDto> UpdateAsync(int id, PersonDto person)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GenderDto>> GetAllGendersAsync()
        {
            var genders = await _repositoryManager.PersonRepository.GetAllGendersAsync();
            return _mapper.Map<IEnumerable<GenderDto>>(genders);
        }
    }
}
