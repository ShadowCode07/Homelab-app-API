using HomelabAPI.Application.DTOs.Devices;
using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Application.Mapping;
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
        private readonly IDeviceRepository _deviceRepository;
        public DeviceService(IDeviceRepository deviceRepository)
            => _deviceRepository = deviceRepository;

        public async Task<DeviceDto> CreateDeviceAsync(DeviceCreateDto dto)
        {
            var device = dto.ToEntity();
            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();
            return device.ToDto();
        }

        public async Task<DeviceDto?> GetDeviceByIdAsync(Guid Id)
            => (await _deviceRepository.GetByIdAsync(Id))?.ToDto();

        public async Task<IReadOnlyList<DeviceDto>> GetDevicesAsync()
            => (await _deviceRepository.GetAllAsync()).ToDtoList();
    }
}
