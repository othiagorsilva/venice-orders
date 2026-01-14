namespace Venice.Orders.Application.DTOs
{
    public record OrderItemDto(
        string Product,
        int Quantity,
        decimal UnitPrice
    );
}