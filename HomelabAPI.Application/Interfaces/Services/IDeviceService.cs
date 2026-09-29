using HomelabAPI.Application.DTOs.Devices;

namespace HomelabAPI.Application.Interfaces.Services
{
    public interface IDeviceService
    {
        Task<IReadOnlyList<DeviceDto>> GetDevicesAsync();
        Task<DeviceDto?> GetDeviceByIdAsync(Guid id);

        Task<DeviceDto> CreateDeviceAsync(DeviceCreateDto dto);
    }
}
