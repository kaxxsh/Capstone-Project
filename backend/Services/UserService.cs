using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.User;
using backend.Model.Dtos.User;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Services
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IUserRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserDto> DeleteUserAsync(string id)
        {
            try
            {
                var result = await repository.Delete(id);
                return mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var result = await repository.GetAll();
                return mapper.Map<IEnumerable<UserDto>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            try
            {
                var result = await repository.GetById(id);
                return mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> GetUserByNameAsync(string Name)
        {
            try
            {
                var result = await repository.GetUserByNameAsync(Name);
                return mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> GetUserByUserNameAsync(string UserName)
        {
            try
            {
                var result = await repository.GetUserByUserNameAsync(UserName);
                return mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> UpdateUserAsync(UserRequestDto user)
        {
            try
            {
                var userId = GetUserId();
                var result = await repository.Update(userId, mapper.Map<UserDetails>(user));
                return mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetUserId()
        {
            var jwtToken = httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            return userId;
        }
    }
}
