using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Dtos.Auth;
using backend.Model.Dtos.User;

namespace backend.Services
{
    public class AuthService : IAuthServices
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<UserResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _authRepository.Login(loginRequestDto);
            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                UserName = user.UserName,
                Name = user.Name,
                ProfileImage = user.ProfileImage,
                Bio = user.Bio,
                Location = user.Location,
                DateOfBirth = user.DateOfBirth,
                JoinDate = user.JoinDate,
                Gender = user.Gender
            };
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var user = await _authRepository.Register(registerRequestDto);
            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                UserName = user.UserName,
                Name = user.Name,
                ProfileImage = user.ProfileImage,
                Bio = user.Bio,
                Location = user.Location,
                DateOfBirth = user.DateOfBirth,
                JoinDate = user.JoinDate,
                Gender = user.Gender
            };
        }
    }
}
