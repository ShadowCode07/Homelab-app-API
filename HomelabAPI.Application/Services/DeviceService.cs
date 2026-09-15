using HomelabAPI.Application.Interfaces;
using HomelabAPI.Core.Entities;
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
                new Device { Id = Guid.NewGuid(), Name = "Device 1", Type = "Type A", Status = "Active" },
                new Device { Id = Guid.NewGuid(), Name = "Device 2", Type = "Type B", Status = "Inactive" },
                new Device { Id = Guid.NewGuid(), Name = "Device 3", Type = "Type C", Status = "Active" }
            });
        }

        public Task<Device> GetDeviceByIdAsync(Guid id)
        {
           return Task.FromResult(new Device { Id = id, Name = "Device 1", Type = "Type A", Status = "Active" });
        }
    }
}
