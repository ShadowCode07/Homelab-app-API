using HomelabAPI.Core.Domain;

namespace HomelabAPI.Application.Interfaces.Repository
{
    public interface IDeviceGroupRepository
    {
        Task<IReadOnlyList<DeviceGroup>> GetAllAsync();
        Task<DeviceGroup?> GetByIdAsync(Guid id);
    }
}
