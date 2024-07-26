using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Dtos.Auth;
using backend.Model.Dtos.Notify;
using backend.Model.Dtos.User;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace backend.Services
{
    public class AuthService : IAuthServices
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotifyServices _notify;

        public AuthService(IAuthRepository authRepository, IMapper mapper, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, INotifyServices notify)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _notify = notify;
        }

        public async Task<UserResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _authRepository.Login(loginRequestDto);
            if (user == null)
            {
                return null;
            }

            var token = _tokenService.CreateToken(user);

            SetTokenCookie(token);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var user = await _authRepository.Register(registerRequestDto);
            if (user == null)
            {
                // Handle registration failure
                return null;
            }

            var token = _tokenService.CreateToken(user);

            SetTokenCookie(token);

            return _mapper.Map<UserResponseDto>(user);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1),
                Secure = true, // Ensure this is true in production to use HTTPS
                SameSite = SameSiteMode.Strict
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", token, cookieOptions);
        }
    }
}
