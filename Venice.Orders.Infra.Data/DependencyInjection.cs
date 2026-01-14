using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Infra.Data.NoSql.Context;
using Venice.Orders.Infra.Data.Repository;
using Venice.Orders.Infrastructure.Persistence.Sql;

namespace Venice.Orders.Infra.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraData(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("SqlServerConnString")));

            services.AddScoped<MongoContext>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Environment.GetEnvironmentVariable("RedisConnString");
                options.InstanceName = "Venice_";
            });

            return services;
        }
    }
}