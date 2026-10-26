using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Core.Domain;

namespace HomelabAPI.Application.Services
{
    public class DeviceGroupService : IDeviceGroupService
    {
        private readonly IDeviceGroupRepository _deviceGroupRepository;

        public DeviceGroupService(IDeviceGroupRepository deviceGroupRepository)
            => _deviceGroupRepository = deviceGroupRepository;

        public async Task<IReadOnlyList<DeviceGroup>> GetDeviceGroupsAsync()
            => await _deviceGroupRepository.GetAllAsync();
    }
}
