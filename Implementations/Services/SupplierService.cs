using MongoDB.Bson;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Interfaces.Services;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;

        public SupplierService(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Supplier> GetByIdAsync(ObjectId id) =>
            await _repository.GetByIdAsync(id);

        public async Task AddAsync(Supplier entity) =>
            await _repository.AddAsync(entity);

        public async Task UpdateAsync(ObjectId id, Supplier entity) =>
            await _repository.UpdateAsync(id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _repository.DeleteAsync(id);

        public async Task<IEnumerable<dynamic>> GetJoinedDataAsync(dynamic payload) =>
            await _repository.JoinMultipleCollectionsAsync(payload);
    }
}
