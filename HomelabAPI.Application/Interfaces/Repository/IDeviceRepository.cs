using HomelabAPI.Core.Entities;

namespace HomelabAPI.Application.Interfaces.Repository
{
    public interface IDeviceRepository : IGenericRepository<Device>
    {
        Task<bool> HostnameExistsAsync(string hostname);
    }
}
