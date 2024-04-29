using Forum.Domain.Entities;

namespace Forum.Application.Topics
{
    public interface ITopicRepository
    {
        Task<List<Topic>> GetAllAsync(CancellationToken cancellationToken);
        Task<Topic> GetAsync(CancellationToken cancellationToken, int id);
        Task<List<Topic>> GetTopicByUser(CancellationToken cancellationToken, string userId);
        Task CreateAsync(CancellationToken cancellationToken, Topic topic);
        Task UpdateAsync(CancellationToken cancellationToken, Topic topic);
        Task<bool> Exists(CancellationToken cancellationToken, int id);

    }
}
