using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task InsertOneAsync(Order salesOrder);
        Task<Order?> FindOneByIdAsync(Guid id);
    }
}