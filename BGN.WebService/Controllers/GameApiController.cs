using AutoMapper;
using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using BGN.Shared;
using BGN.WebService.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace BGN.WebService.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameApiController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;
        public GameApiController(IRepositoryManager manager, IMapper mapper)
        {
            _gameRepository = manager.GameRepository;
            _mapper = mapper;
        }

        [EnableCors("AllowApi")]
        [HttpGet("all")]
        public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
        {
            var games = await _gameRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return dtos;
        }
    }
}
