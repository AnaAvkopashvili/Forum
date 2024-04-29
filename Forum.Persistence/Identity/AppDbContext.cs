using Forum.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Persistence.Identity
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                var entities = ChangeTracker
                    .Entries()
                    .Where(e => e.Entity != null &&
                    (e.State == EntityState.Modified ||
                    e.State == EntityState.Added ||
                    e.State == EntityState.Deleted))
                    .ToList();


                foreach (var entity in entities)
                {
                    entity.State = entity.State switch
                    {
                        EntityState.Added => EntityState.Modified,
                        _ => EntityState.Unchanged,
                    };
                }
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(login => new { login.LoginProvider, login.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(userRole => new { userRole.UserId, userRole.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name });
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(roleClaim => roleClaim.Id);
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(userClaim => userClaim.Id);
        }
    }
}