using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MultipleJoins.Models
{
    public class Supplier
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
