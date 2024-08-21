using MongoDB.Bson;
using MongoDB.Driver;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoCollection<Customer> _collection;

        public CustomerRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Customer>("Customers");
        }

        public async Task<IEnumerable<Customer>> GetAllAsync() =>
            await _collection.Find(Builders<Customer>.Filter.Empty).ToListAsync();

        public async Task<Customer> GetByIdAsync(ObjectId id) =>
            await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();

        public async Task AddAsync(Customer entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(ObjectId id, Customer entity) =>
            await _collection.ReplaceOneAsync(c => c.Id == id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _collection.DeleteOneAsync(c => c.Id == id);

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

            return await _collection.Aggregate<Customer>(pipeline).ToListAsync();
        }
    }
}
