using Forum.Application.Services;
using Forum.Application.Topics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Forum.Web.BackgroundServices
{
    public class TopicArchivalBackgroundService : BackgroundService
    {
        private readonly ILogger<TopicArchivalBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TopicArchivalBackgroundService(ILogger<TopicArchivalBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TopicArchivalBackgroundService is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateAsyncScope();
                    var topicService = scope.ServiceProvider.GetRequiredService<ITopicService>();
                    await topicService.ArchiveInactiveTopicsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while running TopicArchivalBackgroundService.");
                }

                _logger.LogInformation("TopicArchivalBackgroundService ran at: {time}", DateTime.Now);
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}