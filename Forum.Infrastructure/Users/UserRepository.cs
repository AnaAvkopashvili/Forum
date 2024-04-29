using Forum.Application.Accounts;
using Forum.Domain.Entities;
using Forum.Persistence.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Users
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        { }

        public async Task<User> GetAsync(CancellationToken cancellationToken, string id)
        {
            return await base.GetAsync(cancellationToken, id);
        }
        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await base.GetAllAsync(cancellationToken);
        }
        public async Task UpdateAsync(CancellationToken cancellationToken, User user)
        {
            await base.UpdateAsync(cancellationToken, user);
        }
        public async Task<bool> Exists(CancellationToken cancellationToken, string Id)
        {
            return await base.AnyAsync(cancellationToken, x => x.Id == Id);
        }


    }
}
