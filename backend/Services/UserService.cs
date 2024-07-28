using AutoMapper;
using backend.Interface.Repository;
using backend.Interface.Services;
using backend.Model.Domain.User;
using backend.Model.Dtos.User;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                throw;
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
                throw;
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
                throw;
            }
        }

        public async Task<UserDto> GetUserByNameAsync(string name)
        {
            try
            {
                var result = await _repository.GetUserByNameAsync(name);
                return _mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var result = await _repository.GetUserByUserNameAsync(userName);
                return _mapper.Map<UserDto>(result);
            }
            catch (Exception ex)
            {
                throw;
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
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> SearchUserAsync(string search)
        {
            try
            {
                var searchById = await GetUserByIdAsync(search);
                if (searchById != null)
                {
                    return new List<UserDto> { _mapper.Map<UserDto>(searchById) };
                }

                var searchByName = await GetUserByNameAsync(search);
                if (searchByName != null)
                {
                    return new List<UserDto> { _mapper.Map<UserDto>(searchByName) };
                }

                var searchByUserName = await GetUserByUserNameAsync(search);
                if (searchByUserName != null)
                {
                    return new List<UserDto> { _mapper.Map<UserDto>(searchByUserName) };
                }

                return Enumerable.Empty<UserDto>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetUserId()
        {
            try
            {
                var jwtToken = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwtToken))
                {
                    throw new Exception("JWT token is missing.");
                }

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);
                var userId = token.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                return userId ?? throw new Exception("User ID not found in token.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
