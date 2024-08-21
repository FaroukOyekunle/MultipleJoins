using MongoDB.Bson;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Interfaces.Services;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Product> GetByIdAsync(ObjectId id) =>
            await _repository.GetByIdAsync(id);
        
        public async Task AddAsync(Product entity) =>
            await _repository.AddAsync(entity);

        public async Task UpdateAsync(ObjectId id, Product entity) =>
            await _repository.UpdateAsync(id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _repository.DeleteAsync(id);

        public async Task<IEnumerable<dynamic>> GetJoinedDataAsync(dynamic payload) =>
            await _repository.JoinMultipleCollectionsAsync(payload);
    }
}
