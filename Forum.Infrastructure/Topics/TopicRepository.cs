using Forum.Application.Topics;
using Forum.Application.Topics.Responses;
using Forum.Domain.Entities;
using Forum.Persistence.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Topics
{
    public class TopicRepository : BaseRepository<Topic>, ITopicRepository
    {
        public TopicRepository(AppDbContext context) : base(context)
        { }

        public async Task<Topic> GetAsync(CancellationToken cancellationToken, int id)
        {
            return await _dbSet.Include(x => x.User).Include(x => x.Comments).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<Topic>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.Include(x => x.User).
                Include(x => x.Comments).ThenInclude(x => x.User).ToListAsync(cancellationToken);
        }
        public async Task<List<Topic>> GetTopicByUser(CancellationToken cancellationToken, string userId)
        {
            var result = await GetAllAsync(cancellationToken);
            return result.FindAll(x => x.UserId.Equals(userId));
        }
        public async Task CreateAsync(CancellationToken cancellationToken, Topic topic)
        {
            await base.CreateAsync(cancellationToken, topic);
        }
        public async Task UpdateAsync(CancellationToken cancellationToken, Topic topic)
        {
            await base.UpdateAsync(cancellationToken, topic);
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, int id)
        {
            return await base.AnyAsync(cancellationToken, x => x.Id == id);
        }
    }
}
