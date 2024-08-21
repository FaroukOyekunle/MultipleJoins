using MongoDB.Bson;
using MongoDB.Driver;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IMongoCollection<Supplier> _collection;

        public SupplierRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Supplier>("Suppliers");
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync() =>
            await _collection.Find(Builders<Supplier>.Filter.Empty).ToListAsync();

        public async Task<Supplier> GetByIdAsync(ObjectId id) =>
            await _collection.Find(s => s.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Supplier entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(ObjectId id, Supplier entity) =>
            await _collection.ReplaceOneAsync(s => s.Id == id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _collection.DeleteOneAsync(s => s.Id == id);

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

            return await _collection.Aggregate<Supplier>(pipeline).ToListAsync();
        }
    }
}
