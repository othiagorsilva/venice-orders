using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Venice.Orders.Application.Common.Interfaces;

namespace Venice.Orders.Infra.Messaging.Rabbit
{
    public class RabbitEventPublisher : IEventPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly string _exchangeName = "order-events-exchange";

        public RabbitEventPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishAsync<T>(T @event)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: _exchangeName, type: ExchangeType.Fanout, durable: true, autoDelete: false);

            var json = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: _exchangeName, routingKey: string.Empty, body: body);
        }
    }
}