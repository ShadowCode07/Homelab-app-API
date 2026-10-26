using HomelabAPI.Core.Domain;

namespace HomelabAPI.Application.Interfaces.Services
{
    public interface IDeviceGroupService
    {
        Task<IReadOnlyList<DeviceGroup>> GetDeviceGroupsAsync();
    }
}
