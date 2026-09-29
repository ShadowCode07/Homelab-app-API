using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Core.Entities;
using HomelabAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HomelabAPI.Infrastructure.Repostitories
{
    public class DeviceGroupRepository : GenericRepositroy<DeviceGroup>, IDeviceGroupRepository
    {
        public DeviceGroupRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<DeviceGroup>> GetAllAsync()
            => await _set.AsNoTracking().OrderBy(g => g.Name).ToListAsync();
    }
}
