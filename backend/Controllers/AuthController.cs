using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Interface.Services;
using backend.Model.Dtos.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var result = await _authServices.LoginAsync(loginRequestDto);
            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var result = await _authServices.RegisterAsync(registerRequestDto);
            if (result == null)
            {
                return BadRequest("Registration failed");
            }

            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _authServices.LogoutAsync();
            return Ok();
        }
    }
}
