using Venice.Orders.Application.DTOs;

namespace Venice.Orders.Application.UseCases.CreateOrder
{
    public interface ICreateOrderUseCase
    {
        Task<Guid> ExecuteAsync(CreateOrderDto createOrderDto, CancellationToken cancellationToken = default);
    }
}