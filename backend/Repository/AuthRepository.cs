using backend.Interface.Repository;
using backend.Model.Dtos.Auth;
using backend.Model.Domain.User;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using backend.Interface.Services;
using backend.Model.Dtos.Notify;

namespace backend.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<UserDetails> _userManager;
        private readonly SignInManager<UserDetails> _signInManager;
        private readonly INotifyServices _notify;

        public AuthRepository(UserManager<UserDetails> userManager, SignInManager<UserDetails> signInManager, INotifyServices notify)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notify = notify;
        }

        public async Task<UserDetails> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                var notification = new NotifyRequestDto
                {
                    UserId = user.Id,
                    Content = $"New Login Found",
                };

                await _notify.CreateNotificationAsync(notification);
                return user;
            }

            return null;
        }

        public async Task<UserDetails> Register(RegisterRequestDto registerRequestDto)
        {
            var user = new UserDetails
            {
                UserName = registerRequestDto.UserName,
                Name = registerRequestDto.Name,
                ProfileImage = registerRequestDto.ProfileImage,
                Bio = registerRequestDto.Bio,
                Location = registerRequestDto.Location,
                DateOfBirth = registerRequestDto.DateOfBirth,
                Gender = registerRequestDto.Gender,
                Email = registerRequestDto.Email,
                PhoneNumber = registerRequestDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return user;
            }

            return null;
        }
    }
}
