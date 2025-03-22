using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Application.Abstractions.IServices;
using OrderAPI.Infrastructure.Services.Messaging;

namespace OrderAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            //TODO : Cache and logging service will add.
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            return services;
        }
    }
}
