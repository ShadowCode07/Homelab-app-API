using HomelabAPI.Application.Exceptions;
using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Application.Interfaces.Services;
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

        public async Task<Device> CreateDeviceAsync(Device device)
        {
            if (await _deviceRepository.HostnameExistsAsync(device.Hostname))
                throw new DuplicateHostnameException(device.Hostname);

            if (device.DeviceGroupId is Guid groupId)
            {
                var group = await _deviceGroupRepository.GetByIdAsync(groupId)
                    ?? throw new DeviceGroupNotFoundException(groupId);

                device.AssignGroup(group);
            }

            await _deviceRepository.AddAsync(device);
            await _deviceRepository.SaveChangesAsync();

            return device;
        }

        public Task<Device?> GetDeviceByIdAsync(Guid id)
            => _deviceRepository.GetByIdAsync(id);

        public Task<IReadOnlyList<Device>> GetDevicesAsync()
            => _deviceRepository.GetAllAsync();
    }
}
