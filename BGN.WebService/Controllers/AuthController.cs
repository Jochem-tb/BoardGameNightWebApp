using BGN.Domain.Repositories;
using BGN.WebService.Models;
using GraphQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BGN.WebService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPersonRepository _personRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(SignInManager<IdentityUser> signInManager, IRepositoryManager manager, IHttpContextAccessor httpContext)
        {
            _personRepository = manager.PersonRepository;
            _signInManager = signInManager;
            _httpContextAccessor = httpContext;
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> OnLogin([FromBody] LoginModel input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
            {
                return new JsonResult(new { message = "Missing email or password" });
            }

            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var userKeyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var person = await _personRepository.GetPersonIdByUserKey(userKeyId);
                if (person == null)
                {
                    return new JsonResult(new { message = "Something went wrong" });
                }
                _httpContextAccessor.HttpContext.Response.StatusCode = 200;
                return new JsonResult(new { message = "Login successful", UserKey = userKeyId });
            }
            return new JsonResult(new { message = "Invalid login attempt." });

        }
    }
}
