using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MultipleJoins.Models
{
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ObjectId ProductCategoryId { get; set; }
    }
}
