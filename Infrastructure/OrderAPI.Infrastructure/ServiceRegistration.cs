using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Infrastructure.Services.Cache;
using OrderAPI.Infrastructure.Services.MailSender;
using OrderAPI.Infrastructure.Services.Messaging;

namespace OrderAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddSingleton<ICacheService,RedisCacheService>();
            services.AddHostedService<MailSenderBackgroundService>();
            return services;
        }
    }
}
