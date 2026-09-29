using HomelabAPI.Core.Entities;

namespace HomelabAPI.Application.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : BaseClass
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(T entity);
        Task SaveChangesAsync();
    }
}
