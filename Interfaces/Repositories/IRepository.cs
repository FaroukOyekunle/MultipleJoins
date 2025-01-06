using MongoDB.Bson;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MultipleJoins.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task DeleteAsync(ObjectId id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(ObjectId id);
        Task UpdateAsync(ObjectId id, T entity);
        Task<IEnumerable<dynamic>> JoinMultipleCollectionsAsync(dynamic payload);
    }
}
