using OrderAPI.Application.DTOs;

namespace OrderAPI.Application.Abstractions.IServices
{
    public interface IMailService
    {
        Task SendMailAsync(SendMailMessage message);
    }
}
