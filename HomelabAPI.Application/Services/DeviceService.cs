using HomelabAPI.Application.DTOs.Devices;
using HomelabAPI.Application.Exceptions;
using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Application.Mapping;
using HomelabAPI.Core.Domain;

namespace HomelabAPI.Application.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDeviceGroupRepository _deviceGroupRepository;

        public DeviceService(IDeviceRepository deviceRepository, IDeviceGroupRepository deviceGroupRepository)
        {
            _deviceRepository = deviceRepository;
            _deviceGroupRepository = deviceGroupRepository;
        }

        public async Task<DeviceDto> CreateDeviceAsync(DeviceCreateDto dto)
        {
            var device = Device.Create(dto.Name, dto.Hostname, dto.DeviceType, dto.DeviceGroupId);

            if (await _deviceRepository.HostnameExistsAsync(device.Hostname))
                throw new DuplicateHostnameException(device.Hostname);

            string? groupName = null;
            if (device.DeviceGroupId is Guid groupId)
            {
                var group = await _deviceGroupRepository.GetByIdAsync(groupId)
                    ?? throw new DeviceGroupNotFoundException(groupId);
                groupName = group.Name;
            }

            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();

            return device.ToDto(groupName);
        }

        public async Task<DeviceDto?> GetDeviceByIdAsync(Guid id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device is null)
                return null;

            string? groupName = null;
            if (device.DeviceGroupId is Guid groupId)
                groupName = (await _deviceGroupRepository.GetByIdAsync(groupId))?.Name;

            return device.ToDto(groupName);
        }

        public async Task<IReadOnlyList<DeviceDto>> GetDevicesAsync()
        {
            var devices = await _deviceRepository.GetAllAsync();
            var groupNames = (await _deviceGroupRepository.GetAllAsync())
                .ToDictionary(g => g.Id, g => g.Name);

            return devices
                .Select(d => d.ToDto(
                    d.DeviceGroupId is Guid groupId && groupNames.TryGetValue(groupId, out var name) ? name : null))
                .ToList();
        }
    }
}
