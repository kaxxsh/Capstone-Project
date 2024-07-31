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
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<UserResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            try
            {
                var user = await _authRepository.Register(registerRequestDto);
                if (user == null)
                {
                    return null;
                }

                var token = _tokenService.CreateToken(user);

                SetTokenCookie(token);

                return _mapper.Map<UserResponseDto>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task LogoutAsync()
        {
            try
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("jwt");
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(6),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", token, cookieOptions);
        }
    }
}
