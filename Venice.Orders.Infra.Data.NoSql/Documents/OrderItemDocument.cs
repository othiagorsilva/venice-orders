using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Venice.Orders.Infra.Data.NoSql.Documents
{
    public class OrderItemDocument
    {
        public OrderItemDocument() { }

        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid OrderId { get; set; }
        
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}