using MongoDB.Bson;
using MultipleJoins.Implementations.Repositories;
using MultipleJoins.Interfaces.Repositories;
using MultipleJoins.Interfaces.Services;
using MultipleJoins.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Implementations.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Customer> GetByIdAsync(ObjectId id) =>
            await _repository.GetByIdAsync(id);

        public async Task AddAsync(Customer entity) =>
            await _repository.AddAsync(entity);

        public async Task UpdateAsync(ObjectId id, Customer entity) =>
            await _repository.UpdateAsync(id, entity);

        public async Task DeleteAsync(ObjectId id) =>
            await _repository.DeleteAsync(id);

        public async Task<IEnumerable<dynamic>> GetJoinedDataAsync(dynamic payload) =>
            await _repository.JoinMultipleCollectionsAsync(payload);
    }
}
