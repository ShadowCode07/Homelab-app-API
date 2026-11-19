using HomelabAPI.Core.Enums;

namespace HomelabAPI.Application.Interfaces.Services;

public interface IDeviceStatusService
{
    Task<bool> UpdateStatusAsync(Guid deviceId, DeviceStatus status, DateTime observedAt);
}