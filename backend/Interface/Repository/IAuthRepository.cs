using backend.Model.Dtos.Auth;
using backend.Model.Domain.User;
using System.Threading.Tasks;

namespace backend.Interface.Repository
{
    public interface IAuthRepository
    {
        Task<UserDetails> Login(LoginRequestDto loginRequestDto);
        Task<UserDetails> Register(RegisterRequestDto registerRequestDto);
    }
}
