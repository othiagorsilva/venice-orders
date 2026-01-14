using Venice.Orders.Domain.Exceptions;

namespace Venice.Orders.Domain.Entities
{
    public class OrderItem
    {
                public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public string Product { get; private set; } = default!;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        private OrderItem(Guid id, Guid salesOrderId, string product, int quantity, decimal unitPrice)
        {
            Id = id;
            OrderId = salesOrderId;
            Product = product;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public OrderItem(Guid salesOrderId, string product, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new DomainException("Quantidade deve ser maior que zero.");
            if (unitPrice <= 0) throw new DomainException("Preço deve ser positivo.");

            OrderId = salesOrderId;
            Product = product;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public decimal CalculateTotal() => UnitPrice * Quantity;

        public static OrderItem Load(Guid id, Guid orderId, string product, int quantity, decimal unitPrice)
        {
            return new OrderItem(id, orderId, product, quantity, unitPrice);
        }
    }
}