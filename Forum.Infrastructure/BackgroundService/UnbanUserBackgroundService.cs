using Forum.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Forum.Web.BackgroundServices
{
    public class UnbanUserBackgroundService : BackgroundService
    {
        private readonly ILogger<UnbanUserBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public UnbanUserBackgroundService(ILogger<UnbanUserBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BanUserBackgroundService is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateAsyncScope();
                    var _adminService = scope.ServiceProvider.GetRequiredService<IAdminService>();
                    var users = await _adminService.GetAllBannedAsync();

                    foreach (var user in users)
                    {
                        await _adminService.UnbanUserAsync(user.Id);
                        _logger.LogInformation($"User {user.UserName} has been unbanned.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while running BanUserBackgroundService.");
                }

                _logger.LogInformation("BanUserBackgroundService ran at: {time}", DateTime.Now);
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
