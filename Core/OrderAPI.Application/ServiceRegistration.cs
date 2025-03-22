using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Application.Mapping;

namespace OrderAPI.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
