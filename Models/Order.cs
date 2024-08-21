using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace MultipleJoins.Models
{
    public class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId CustomerId { get; set; }
        public ObjectId ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        [BsonIgnore]
        public Customer Customer { get; set; }
        [BsonIgnore]
        public Product Product { get; set; }
    }
}
