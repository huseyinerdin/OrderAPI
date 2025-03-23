using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Application.DTOs;

namespace OrderAPI.Infrastructure.Services.MailSender
{
    public class MailSenderBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<MailSenderBackgroundService> _logger;

        public MailSenderBackgroundService(IServiceProvider serviceProvider, IRabbitMqService rabbitMqService, ILogger<MailSenderBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _rabbitMqService.Consume<SendMailMessage>("SendMail", async (message) =>
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                        await mailService.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Mail gönderimi sırasında hata oluştu.");
                    }
                });
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
