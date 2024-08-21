using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultipleJoins.Interfaces.Services
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(ObjectId id);
        Task AddAsync(T entity);
        Task UpdateAsync(ObjectId id, T entity);
        Task DeleteAsync(ObjectId id);
        Task<IEnumerable<dynamic>> GetJoinedDataAsync(dynamic payload);
    }
}
