using HomelabAPI.Core.Domain;

namespace HomelabAPI.Application.Interfaces.Services
{
    public interface IDeviceService
    {
        Task<IReadOnlyList<Device>> GetDevicesAsync();
        Task<Device?> GetDeviceByIdAsync(Guid id);

        Task<Device> CreateDeviceAsync(Device device);
    }
}
