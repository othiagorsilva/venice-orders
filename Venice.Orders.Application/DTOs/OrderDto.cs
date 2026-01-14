namespace Venice.Orders.Application.DTOs
{
    public record OrderDto(
        Guid Id,
        Guid CustomerId,
        DateTime CreatedAt,
        int Status,
        IEnumerable<OrderItemDto> Items
    );
}