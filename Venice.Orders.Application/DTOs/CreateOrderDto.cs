namespace Venice.Orders.Application.DTOs
{
    public record CreateOrderDto(
        Guid CustomerId,
        List<OrderItemDto> Items
    );
}