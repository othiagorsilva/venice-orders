using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Venice.Orders.Infra.Data.NoSql.Documents;

namespace Venice.Orders.Infra.Data.NoSql.Context
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

    public MongoContext(IConfiguration configuration)
    {
        var client = new MongoClient(Environment.GetEnvironmentVariable("MongoConnString"));
        _database = client.GetDatabase(Environment.GetEnvironmentVariable("MongoDatabase"));
    }

    public IMongoCollection<OrderItemDocument> OrderItems =>
        _database.GetCollection<OrderItemDocument>("OrderItems");
    }
}