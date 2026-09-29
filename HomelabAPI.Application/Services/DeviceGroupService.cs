using HomelabAPI.Application.DTOs.DeviceGroups;
using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Application.Mapping;

namespace HomelabAPI.Application.Services
{
    public class DeviceGroupService : IDeviceGroupService
    {
        private readonly IDeviceGroupRepository _deviceGroupRepository;

        public DeviceGroupService(IDeviceGroupRepository deviceGroupRepository)
            => _deviceGroupRepository = deviceGroupRepository;

        public async Task<IReadOnlyList<DeviceGroupDto>> GetDeviceGroupsAsync()
            => (await _deviceGroupRepository.GetAllAsync()).ToDtoList();
    }
}
