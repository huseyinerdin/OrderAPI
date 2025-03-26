using Microsoft.Extensions.Logging;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Application.DTOs;

namespace OrderAPI.Infrastructure.Services.MailSender
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;

        public MailService(ILogger<MailService> logger)
        {
            _logger = logger;
        }

        public Task SendMailAsync(SendMailMessage message)
        {
            _logger.LogInformation("📧 Mail Gönderildi -> {To} | Konu: {Subject} | İçerik: {Body}", message.To, message.Subject, message.Body);
            return Task.CompletedTask;
        }
    }
}
