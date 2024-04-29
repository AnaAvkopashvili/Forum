using Forum.Application.Comments;
using Forum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Forum.Persistence.Identity;


namespace Forum.Infrastructure.Comments
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context) { }

        public async Task<Comment> GetAsync(CancellationToken cancellationToken, int id)
        {
            return await _dbSet.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.Include(x => x.User).ToListAsync();
        }
        public async Task CreateAsync(CancellationToken cancellationToken, Comment comment)
        {
            await base.CreateAsync(cancellationToken, comment);
        }
        public async Task UpdateAsync(CancellationToken cancellationToken, Comment comment)
        {
            await base.UpdateAsync(cancellationToken, comment);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken, Comment comment)
        {
            comment.IsDeleted = true;
            await UpdateAsync(cancellationToken, comment);
        }

        public async Task<bool> Exists(CancellationToken cancellationToken, int id)
        {
            return await base.AnyAsync(cancellationToken, x => x.Id == id);
        }

    }
}
