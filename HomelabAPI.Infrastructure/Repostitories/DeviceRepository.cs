using HomelabAPI.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Infrastructure.Repostitories
{
    public class DeviceRepository : GenericRepositroy, IDeviceRepository
    {
        public DeviceRepository() { 
        }
    }
}
