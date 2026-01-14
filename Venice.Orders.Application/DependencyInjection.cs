using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Application.UseCases.CreateOrder;
using Venice.Orders.Application.UseCases.GetOrder;

namespace Venice.Orders.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
            services.AddScoped<IGetOrderUseCase, GetOrderUseCase>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}