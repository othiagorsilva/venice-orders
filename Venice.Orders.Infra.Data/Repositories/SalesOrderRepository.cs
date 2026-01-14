using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Repositories;
using Venice.Orders.Infra.Data.NoSql.Context;
using Venice.Orders.Infra.Data.NoSql.Documents;
using Venice.Orders.Infrastructure.Persistence.Sql;

namespace Venice.Orders.Infra.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _sqlContext;
        private readonly MongoContext _noSqlContext;

        public OrderRepository(AppDbContext sqlContext, MongoContext noSqlContext)
        {
            _sqlContext = sqlContext;
            _noSqlContext = noSqlContext;
        }

        public async Task InsertOneAsync(Order entity)
        {
            await _sqlContext.Orders.AddAsync(entity);
            await _sqlContext.SaveChangesAsync();

            var docs = entity.Items.Select(item => MapToDocument(item)).ToList();

            await _noSqlContext.OrderItems.InsertManyAsync(docs);
        }

        public async Task<Order?> FindOneByIdAsync(Guid id)
        {
            var order = await  _sqlContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
                return null;

            var documents = await _noSqlContext.OrderItems.Find(item => item.OrderId == order.Id).ToListAsync();
            var items = documents?.Select(doc => MapToDomain(doc)) ?? Enumerable.Empty<OrderItem>();

            order.LoadItems(items);

            return order;
        }

        private OrderItemDocument MapToDocument(OrderItem item) => new()
        {
            Id = item.Id,
            OrderId = item.OrderId,
            Product = item.Product,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        };

        private OrderItem MapToDomain(OrderItemDocument doc) => 
            OrderItem.Load(doc.Id, doc.OrderId, doc.Product, doc.Quantity, doc.UnitPrice);
    }
}