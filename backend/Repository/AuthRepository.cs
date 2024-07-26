using backend.Interface.Repository;
using backend.Model.Dtos.Auth;
using backend.Model.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace backend.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<UserDetails> _userManager;
        private readonly SignInManager<UserDetails> _signInManager;

        public AuthRepository(UserManager<UserDetails> userManager, SignInManager<UserDetails> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserDetails> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
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
