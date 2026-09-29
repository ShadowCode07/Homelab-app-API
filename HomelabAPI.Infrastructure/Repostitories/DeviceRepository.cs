using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Core.Entities;
using HomelabAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HomelabAPI.Infrastructure.Repostitories
{
    public class DeviceRepository : GenericRepositroy<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Device>> GetAllAsync()
            => await _set
                .AsNoTracking()
                .Include(d => d.DeviceGroup)
                .OrderBy(d => d.Name)
                .ToListAsync();

        public override Task<Device?> GetByIdAsync(Guid id)
            => _set
                .Include(d => d.DeviceGroup)
                .FirstOrDefaultAsync(d => d.Id == id);

        public Task<bool> HostnameExistsAsync(string hostname)
            => _set.AnyAsync(d => d.Hostname == hostname);
    }
}
