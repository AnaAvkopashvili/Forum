namespace Forum.Application
{
    public interface IUnitofWork : IDisposable
    {
        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
