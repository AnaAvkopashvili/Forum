/*using Forum.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Forum.BackgroundWorker
{
    public class UserBanBackgroundService : BackgroundService
    {
        private readonly ILogger<UserBanBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public UserBanBackgroundService(ILogger<UserBanBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(CheckForExpiredBans, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private async void CheckForExpiredBans(object state)
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var bannedUsers = await userManager.Users.Where(u => u.BanExpiration.HasValue && u.BanExpiration.Value < DateTime.UtcNow).ToListAsync();

            foreach (var user in bannedUsers)
            {
                user.BanExpiration = null;
                await userManager.UpdateAsync(user);
                _logger.LogInformation($"Ban removed for user: {user.UserName}");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            await base.StopAsync(cancellationToken);
        }
    }
}*/