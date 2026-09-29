using HomelabAPI.Application.DTOs.Devices;
using HomelabAPI.Application.Exceptions;
using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Application.Mapping;

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
            var device = dto.ToEntity();

            device.Name = device.Name.Trim();
            device.Hostname = NormalizeHostname(device.Hostname);
            device.IpAddress = string.IsNullOrWhiteSpace(device.IpAddress) ? null : device.IpAddress.Trim();

            if (await _deviceRepository.HostnameExistsAsync(device.Hostname))
                throw new DuplicateHostnameException(device.Hostname);

            if (device.DeviceGroupId is Guid groupId && !await _deviceGroupRepository.ExistsAsync(groupId))
                throw new DeviceGroupNotFoundException(groupId);

            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();

            var created = await _deviceRepository.GetByIdAsync(device.Id) ?? device;
            return created.ToDto();
        }

        public async Task<DeviceDto?> GetDeviceByIdAsync(Guid id)
            => (await _deviceRepository.GetByIdAsync(id))?.ToDto();

        public async Task<IReadOnlyList<DeviceDto>> GetDevicesAsync()
            => (await _deviceRepository.GetAllAsync()).ToDtoList();

        public static string NormalizeHostname(string hostname)
            => hostname.Trim().ToLowerInvariant();
    }
}
