using Forum.Application;
using Forum.Persistence.Identity;

namespace Forum.Infrastructure.UOW
{
    public class UnitofWork : IUnitofWork
    {
        protected readonly AppDbContext _context;
        public UnitofWork(AppDbContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
