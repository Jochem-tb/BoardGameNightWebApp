using AutoMapper;
using BGN.Domain.Entities;
using BGN.Domain.Repositories;
using BGN.Infrastructure.Repositories;
using BGN.Shared;
using BGN.WebService.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Tokens;

namespace BGN.WebService.Controllers
{
    [ApiController]
    [Route("api/gamenight")]
    public class GameNightApiController : Controller
    {
        private readonly IGameNightRepository _gameNightRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GameNightApiController(IRepositoryManager manager, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _gameNightRepository = manager.GameNightRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContext;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<GameNightDto>> GetAllGameNightsAsync()
        {
            var gameNights = await _gameNightRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<GameNightDto>>(gameNights);
            return dtos;
        }

        [HttpGet("join/{id}")]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        public async Task<IActionResult> JoinGameNightAsync(string id)
        {
            if (!int.TryParse(id, out int gameNightId))
            {
                return new JsonResult(new { message = "Invalid GameNight ID. Please provide a valid integer." });
            }
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue(AuthConstants.UserKeyHeader, out var userKeyHeader);
            if (userKeyHeader.IsNullOrEmpty())
            {
                return new JsonResult(new { message = $"Invalid header value for: {AuthConstants.UserKeyHeader}" });
            }

            var personCanJoinThisGameNight = await CheckJoinConstraintsAsync(gameNightId, userKeyHeader);

            if(personCanJoinThisGameNight)
            {
                bool success = await _gameNightRepository.JoinGameNightAsync(gameNightId, userKeyHeader);

                if (success)
                {
                    var gameNight = await _gameNightRepository.GetByIdAsync(gameNightId);
                    return new JsonResult(new { message = "Successfully joined GameNight", Attendees = gameNight.Attendees });
                }

                return new JsonResult(new { message = "Something went wrong while joining GameNight" });
            }
            else
            {
                return new JsonResult(new { message = "Something went wrong while joining GameNight", possibleReasons = new List<string>() {"Already attending", "Already attending other gameNight same day", "No place available" } });
            }
            
        }

        private async Task<bool> CheckJoinConstraintsAsync(int gameNightId, string userKeyHeader)
        {
            var gameNight = await _gameNightRepository.GetByIdAsync(gameNightId);
            if (gameNight == null)
            {
                return false;
            }

            //Check if Attending == MaxPlayers
            //var isPlaceAvailable = gameNight.Where(x => x.Attendees.Count() < x.MaxPlayers).Any();
            var isPlaceAvailable = gameNight!.Attendees.Count < gameNight.MaxPlayers;
            if (!isPlaceAvailable)
            {
                return false;
            }

            //Check if person is already attending
            //var isAttending = gameNight.Where(x => x.Attendees.Any(y => y.IdentityUserId == identityUserKey)).Any();
            var isAttending = gameNight.Attendees.Any(y => y.Attendee.IdentityUserId == userKeyHeader);
            if (isAttending)
            {
                return false;
            }

            //Organiser can join his own game, only once as per check above

            //Check if user already has a game night on the same date
            var isAlreadyAttendingAnotherGameNightSameDay =  _gameNightRepository.GetAllGameNightsAsQueryableAsync().Result
                .Where(x => x.Attendees.Any(y => y.Attendee.IdentityUserId == userKeyHeader))
                .Any(x => x.Date == gameNight.Date);
            if (isAlreadyAttendingAnotherGameNightSameDay)
            {
                return false;
            }

            return true;
        }
    }
}

