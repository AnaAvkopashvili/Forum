using Forum.Domain.Entities;

namespace Forum.Application.Accounts
{
    public interface IUserRepository
    {
        Task<User> GetAsync(CancellationToken cancellationToken, string id);
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(CancellationToken cancellationToken, User user);
        Task<bool> Exists(CancellationToken cancellationToken, string email);
    }
}
