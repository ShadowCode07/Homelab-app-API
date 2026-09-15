using HomelabAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Application.Interfaces
{
    public interface IDeviceService
    {
        Task<List<Device>> GetDevicesAsync();
        Task<Device> GetDeviceByIdAsync(Guid Id);
    }
}
