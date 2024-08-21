using MongoDB.Bson;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Interfaces.Services;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Order>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Order> GetByIdAsync(ObjectId id) =>
            await _repository.GetByIdAsync(id);

        public async Task AddAsync(Order entity) =>
            await _repository.AddAsync(entity);

        public async Task UpdateAsync(ObjectId id, Order entity) =>
            await _repository.UpdateAsync(id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _repository.DeleteAsync(id);

        public async Task<IEnumerable<dynamic>> GetJoinedDataAsync(dynamic payload) =>
            await _repository.JoinMultipleCollectionsAsync(payload);
    }
}
