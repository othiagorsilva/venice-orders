using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Application.Common.Interfaces;
using Venice.Orders.Infra.Messaging.Rabbit;

namespace Venice.Orders.Infra.Messaging
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddScoped<IEventPublisher, RabbitEventPublisher>();

            return services;
        }
    }
}