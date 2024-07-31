using backend.Model.Dtos.Auth;
using backend.Model.Dtos.User;

namespace backend.Interface.Services
{
    public interface IAuthServices
    {
        Task<UserResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<UserResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task LogoutAsync();
    }
}
