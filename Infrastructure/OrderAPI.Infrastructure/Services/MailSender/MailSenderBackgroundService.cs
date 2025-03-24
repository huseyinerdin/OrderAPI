using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Application.DTOs;

namespace OrderAPI.Infrastructure.Services.MailSender
{
    public class MailSenderBackgroundService : BackgroundService
    {
        private readonly IMailService _mailService;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<MailSenderBackgroundService> _logger;

        public MailSenderBackgroundService(IMailService mailService, IRabbitMqService rabbitMqService, ILogger<MailSenderBackgroundService> logger)
        {
            _logger = logger;
            _mailService = mailService;
            _rabbitMqService = rabbitMqService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _rabbitMqService.Consume<SendMailMessage>("SendMail", async (message) =>
            {
               try
               {
                   await _mailService.SendMailAsync(message);
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, "An error occurred while sending the email.");
               }
            });
            return Task.CompletedTask;
        }
    }
}
