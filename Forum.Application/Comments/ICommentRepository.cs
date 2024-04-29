using Forum.Domain.Entities;

namespace Forum.Application.Comments
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken);
        Task<Comment> GetAsync(CancellationToken cancellationToken, int id);
        Task CreateAsync(CancellationToken cancellationToken, Comment comment);
        Task UpdateAsync(CancellationToken cancellationToken, Comment comment);
        Task DeleteAsync(CancellationToken cancellationToken, Comment comment);
        Task<bool> Exists(CancellationToken cancellationToken, int id);
    }
}
