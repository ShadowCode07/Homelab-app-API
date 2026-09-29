using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Core.Entities;
using HomelabAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Application.Services
{
    public class DeviceService : IDeviceService
    {
        public Task<List<Device>> GetDevicesAsync()
        {
            return Task.FromResult(new List<Device>
            {
                new Device { Id = Guid.NewGuid(), Name = "Device 1", DeviceType = DeviceType.Server, DeviceStatus = DeviceStatus.Online },
                new Device { Id = Guid.NewGuid(), Name = "Device 2", DeviceType = DeviceType.Mobile, DeviceStatus = DeviceStatus.Offline },
                new Device { Id = Guid.NewGuid(), Name = "Device 3", DeviceType = DeviceType.Computer, DeviceStatus = DeviceStatus.Online }
            });
        }

        public Task<Device> GetDeviceByIdAsync(Guid id)
        {
           return Task.FromResult(new Device { Id = id, Name = "Device 1", DeviceType = DeviceType.Server, DeviceStatus = DeviceStatus.Online });
        }
    }
}
