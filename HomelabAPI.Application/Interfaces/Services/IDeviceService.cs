using HomelabAPI.Application.DTOs.Devices;
using HomelabAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Application.Interfaces.Services
{
    public interface IDeviceService
    {
        Task<IReadOnlyList<DeviceDto>> GetDevicesAsync();
        Task<DeviceDto?> GetDeviceByIdAsync(Guid Id);
        Task<DeviceDto> CreateDeviceAsync(DeviceCreateDto dto);
    }
}
