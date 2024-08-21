using MongoDB.Bson;
using MongoDB.Driver;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly IMongoCollection<ProductCategory> _collection;

        public ProductCategoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ProductCategory>("ProductCategories");
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync() =>
            await _collection.Find(Builders<ProductCategory>.Filter.Empty).ToListAsync();

        public async Task<ProductCategory> GetByIdAsync(ObjectId id) =>
            await _collection.Find(pc => pc.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(ProductCategory entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(ObjectId id, ProductCategory entity) =>
            await _collection.ReplaceOneAsync(pc => pc.Id == id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _collection.DeleteOneAsync(pc => pc.Id == id);

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
                    { "a", lookup.Alias }
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

            return await _collection.Aggregate<ProductCategory>(pipeline).ToListAsync();
        }
    }
}
