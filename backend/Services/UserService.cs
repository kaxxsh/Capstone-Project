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
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserDto> DeleteUserAsync(string id)
        {
            try
            {
                var result = await _repository.Delete(id);
                return _mapper.Map<UserDto>(result);
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
                var result = await _repository.GetAll();
                return _mapper.Map<IEnumerable<UserDto>>(result);
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
                var result = await _repository.GetById(id);
                return _mapper.Map<UserDto>(result);
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
                var result = await _repository.GetUserByNameAsync(Name);
                return _mapper.Map<UserDto>(result);
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
                var result = await _repository.GetUserByUserNameAsync(UserName);
                return _mapper.Map<UserDto>(result);
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
                var result = await _repository.Update(userId, _mapper.Map<UserDetails>(user));
                return _mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetUserId()
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            return userId;
        }
    }
}
