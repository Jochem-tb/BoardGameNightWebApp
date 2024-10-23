using AutoMapper;
using BGN.Domain.Repositories;
using BGN.Services.Abstractions;
using BGN.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<PersonDto?> GetLoggedInUserAsync()
        {
            //Contact the httpContext for the logged in user and return the Id 
            var identityUserKey = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(identityUserKey == null)
            {
                return null;
            }
            //Ger PersonDto 
            var person = await _repositoryManager.PersonRepository.GetPersonIdByUserKey(identityUserKey);
            if (person == null)
            {
                return null;
            }
            return _mapper.Map<PersonDto>(person);
        }
    }
}
