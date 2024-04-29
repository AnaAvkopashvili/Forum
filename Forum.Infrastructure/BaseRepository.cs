using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Forum.Infrastructure
{
    public class BaseRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<T> GetAsync(CancellationToken cancellationToken, params object[] key)
        {
            return await _dbSet.FindAsync(key, cancellationToken);
        }

        public async Task CreateAsync(CancellationToken cancellationToken, T entity)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(CancellationToken cancellationToken, T entity)
        {
            if (entity == null)
            {
                return;
            }
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(CancellationToken cancellationToken, T entity)
        {
            if (entity == null)
            {
                return;
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(CancellationToken cancellationToken, params object[] key)
        {
            var entity = await GetAsync(cancellationToken, key);
            if (entity == null)
            {
                return;
            }
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate, cancellationToken);
        }
    }
}
