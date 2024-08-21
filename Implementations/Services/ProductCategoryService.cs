using MongoDB.Bson;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Interfaces.Services;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _repository;
        public ProductCategoryService(IProductCategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ProductCategory>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<ProductCategory> GetByIdAsync(ObjectId id) =>
            await _repository.GetByIdAsync(id);

        public async Task AddAsync(ProductCategory entity)
        {
            if (entity.Id == ObjectId.Empty)
            {
                entity.Id = ObjectId.GenerateNewId();
            }

            foreach (var product in entity.Products)
            {
                if (product.Id == ObjectId.Empty)
                {
                    product.Id = ObjectId.GenerateNewId();
                }
                product.ProductCategoryId = entity.Id;
            }

            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(ObjectId id, ProductCategory entity) =>
            await _repository.UpdateAsync(id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _repository.DeleteAsync(id);

        public async Task<IEnumerable<dynamic>> GetJoinedDataAsync(dynamic payload) =>
            await _repository.JoinMultipleCollectionsAsync(payload);
    }
}
