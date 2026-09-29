using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Core.Entities;
using HomelabAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task CreateAsync(T entity)
            => await _set.AddAsync(entity);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _set.ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id)
            => await _set.FindAsync(id).AsTask();

    }
}
