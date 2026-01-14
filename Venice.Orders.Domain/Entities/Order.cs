using Venice.Orders.Domain.Enums;
using Venice.Orders.Domain.Exceptions;

namespace Venice.Orders.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal TotalAmount { get; private set; }

        private List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        protected Order() { }
        public Order(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            Status = OrderStatus.Created;
        }

        public void AddItem(string product, int quantity, decimal unitPrice)
        {
            if (_items.Any(x => x.Product == product))
            {
                throw new DomainException("Produto já adicionado ao pedido.");
            }

            _items.Add(new OrderItem(this.Id, product, quantity, unitPrice));

            RecalculateTotal();
        }

        public void LoadItems(IEnumerable<OrderItem> items)
        {
            _items.Clear();
            _items.AddRange(items);
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Sum(x => x.CalculateTotal());
        }
    }
}