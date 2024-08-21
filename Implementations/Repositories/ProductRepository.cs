using MongoDB.Bson;
using MongoDB.Driver;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Models;
using SharpCompress.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Product>("Products");
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _collection.Find(Builders<Product>.Filter.Empty).ToListAsync();

        public async Task<Product> GetByIdAsync(ObjectId id) =>
            await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Product entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(ObjectId id, Product entity) =>
        await _collection.ReplaceOneAsync(p => p.Id == id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _collection.DeleteOneAsync(p => p.Id == id);

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

            return await _collection.Aggregate<Product>(pipeline).ToListAsync();
        }
    }
}
