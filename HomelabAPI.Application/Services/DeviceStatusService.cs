using HomelabAPI.Application.Interfaces.Repository;
using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Core.Enums;

namespace HomelabAPI.Application.Services;

public class DeviceStatusService : IDeviceStatusService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceStatusService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }
    
    public async Task<bool> UpdateStatusAsync(Guid deviceId, DeviceStatus status, DateTime observedAt)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);

        if (device == null)
            return false;
        
        device.UpdateStatus(status, observedAt);
        
        await _deviceRepository.UpdateAsync(device);
        await _deviceRepository.SaveChangesAsync();
        
        return true;
    }
}