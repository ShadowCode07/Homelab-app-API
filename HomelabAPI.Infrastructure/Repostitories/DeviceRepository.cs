using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Core.Entities;
using HomelabAPI.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Infrastructure.Repostitories
{
    public class DeviceRepository : GenericRepositroy<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
