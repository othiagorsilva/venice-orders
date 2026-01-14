using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Venice.Orders.Application.DTOs;
using Venice.Orders.Domain.Repositories;

namespace Venice.Orders.Application.UseCases.GetOrder
{
    public class GetOrderUseCase : IGetOrderUseCase
    {
        private readonly IOrderRepository _repository;
        private readonly IDistributedCache _cache;

        public GetOrderUseCase(IOrderRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<OrderDto?> ExecuteAsync(Guid id, CancellationToken cancellationToken)
        {
            var cacheKey = $"order:{id}";

            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<OrderDto>(cachedData);
            }

            var order = await _repository.FindOneByIdAsync(id);
            if (order == null)
                return null;

            var orderDto = new OrderDto(
                order.Id,
                order.CustomerId,
                order.CreatedAt,
                Convert.ToInt16(order.Status),
                order.Items.Select(item => new OrderItemDto(
                    item.Product,
                    item.Quantity,
                    item.UnitPrice
                ))
            );

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            };

            var serializedData = JsonSerializer.Serialize(orderDto);
            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions, cancellationToken);

            return orderDto;
        }
    }
}