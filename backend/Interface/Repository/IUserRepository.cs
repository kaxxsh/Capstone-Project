using backend.Model.Domain.User;

namespace backend.Interface.Repository
{
    public interface IUserRepository: IRepository<string,UserDetails>
    {
        Task<UserDetails> GetUserByNameAsync(string Name);
        Task<UserDetails> GetUserByUserNameAsync(string UserName);
    }
}
