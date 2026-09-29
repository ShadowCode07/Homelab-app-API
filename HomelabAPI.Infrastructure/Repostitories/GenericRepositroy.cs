using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Core.Entities;
using HomelabAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HomelabAPI.Infrastructure.Repostitories
{
    public abstract class GenericRepositroy<T> : IGenericRepository<T> where T : BaseClass
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _set;

        protected GenericRepositroy(ApplicationDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
            => await _set.AddAsync(entity);

        public virtual async Task<IEnumerable<T>> GetAllAsync()
            => await _set.AsNoTracking().ToListAsync();

        public virtual async Task<T?> GetByIdAsync(Guid id)
            => await _set.FindAsync(id).AsTask();

        public virtual Task<bool> ExistsAsync(Guid id)
            => _set.AnyAsync(e => e.Id == id);

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}
