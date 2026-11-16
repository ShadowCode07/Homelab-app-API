using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Core.Domain;
using HomelabAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HomelabAPI.Infrastructure.Repostitories
{
    public class DeviceGroupRepository : IDeviceGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<DeviceGroup>> GetAllAsync()
        {
            var entities = await _context.DeviceGroups
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .ToListAsync();

            return entities.Select(e => e.ToDomain()).ToList();
        }

        public async Task<DeviceGroup?> GetByIdAsync(Guid id)
        {
            var entity = await _context.DeviceGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            return entity?.ToDomain();
        }
    }
}
