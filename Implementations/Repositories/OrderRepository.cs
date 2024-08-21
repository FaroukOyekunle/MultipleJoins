using MongoDB.Bson;
using MongoDB.Driver;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _collection;

        public OrderRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Order>("Orders");
        }

        public async Task<IEnumerable<Order>> GetAllAsync() =>
            await _collection.Find(Builders<Order>.Filter.Empty).ToListAsync();

        public async Task<Order> GetByIdAsync(ObjectId id) =>
            await _collection.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Order entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(ObjectId id, Order entity) =>
            await _collection.ReplaceOneAsync(o => o.Id == id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _collection.DeleteOneAsync(o => o.Id == id);

        public async Task<IEnumerable<dynamic>> JoinMultipleCollectionsAsync(dynamic payload)
        {
            var pipeline = new List<BsonDocument>();

            foreach (var lookup in payload.LookUpList)
            {
                pipeline.Add(new BsonDocument("$lookup",
                    new BsonDocument
                    {
                    { "from", lookup.Collection },
                    { "localField", lookup.LocalField },
                    { "foreignField", lookup.ForeignField },
                    { "as", lookup.Alias }
                    }
                ));

                if (lookup.Unwind)
                {
                    pipeline.Add(new BsonDocument("$unwind", "$" + lookup.Alias));
                }
            }

            if (payload.Project != null)
            {
                var projectFields = new BsonDocument();
                foreach (var field in payload.Project)
                {
                    projectFields.Add(field, 1);
                }
                pipeline.Add(new BsonDocument("$project", projectFields));
            }

            return await _collection.Aggregate<Order>(pipeline).ToListAsync();
        }
    }
}
