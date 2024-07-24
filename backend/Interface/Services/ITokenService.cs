using backend.Model.Domain.User;

namespace backend.Interface.Services
{
    public interface ITokenService
    {
        string CreateToken(UserDetails user);
    }
}
