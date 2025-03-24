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
            //TODO : Mail gönderme işlemi yapılacak
            _logger.LogWarning($"📧 Mail Gönderildi -> {message.To} | Konu: {message.Subject} | İçerik: {message.Body}");
            return Task.CompletedTask;
        }
    }
}
