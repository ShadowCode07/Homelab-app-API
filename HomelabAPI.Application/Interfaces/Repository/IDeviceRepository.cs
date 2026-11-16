using HomelabAPI.Core.Domain;

namespace HomelabAPI.Application.Interfaces.Repository
{
    public interface IDeviceRepository
    {
        Task<IReadOnlyList<Device>> GetAllAsync();
        Task<Device?> GetByIdAsync(Guid id);
        Task<bool> HostnameExistsAsync(string hostname);
        Task AddAsync(Device device);
        Task SaveChangesAsync();
    }
}
