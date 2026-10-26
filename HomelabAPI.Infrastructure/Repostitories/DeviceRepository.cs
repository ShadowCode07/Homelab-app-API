using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Core.Domain;
using HomelabAPI.Infrastructure.Data;
using HomelabAPI.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomelabAPI.Infrastructure.Repostitories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Device>> GetAllAsync()
        {
            var entities = await _context.Devices
                .AsNoTracking()
                .Include(d => d.DeviceGroup)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return entities.Select(e => e.ToDomain()).ToList();
        }

        public async Task<Device?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Devices
                .AsNoTracking()
                .Include(d => d.DeviceGroup)
                .FirstOrDefaultAsync(d => d.Id == id);

            return entity?.ToDomain();
        }

        public Task<bool> HostnameExistsAsync(string hostname)
            => _context.Devices.AnyAsync(d => d.Hostname == hostname);

        public async Task AddAsync(Device device)
            => await _context.Devices.AddAsync(DeviceEntity.FromDomain(device));

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}
