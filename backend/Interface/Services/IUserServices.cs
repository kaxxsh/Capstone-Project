using backend.Model.Domain.User;
using backend.Model.Dtos.User;

namespace backend.Interface.Services
{
    public interface IUserServices
    {
        Task<UserDto> GetUserByIdAsync(string id);
        Task<UserDto> GetUserByNameAsync(string Name);
        Task<UserDto> GetUserByUserNameAsync(string UserName);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(UserRequestDto user);
        Task<UserDto> DeleteUserAsync(string id);
        Task<IEnumerable<UserDto>> SearchUserAsync(string Search);
    }
}
