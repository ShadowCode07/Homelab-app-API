using HomelabAPI.Application.DTOs.DeviceGroups;

namespace HomelabAPI.Application.Interfaces.Services
{
    public interface IDeviceGroupService
    {
        Task<IReadOnlyList<DeviceGroupDto>> GetDeviceGroupsAsync();
    }
}
