using HomelabAPI.Application.DTOs.Devices;
using HomelabAPI.Core.Domain;

namespace HomelabAPI.Application.Mapping
{
    public static class DeviceMapper
    {
        public static DeviceDto ToDto(this Device device, string? deviceGroupName)
            => new(
                device.Id,
                device.Name,
                device.Hostname,
                device.DeviceType,
                device.DeviceStatus,
                device.IpAddress,
                device.DeviceGroupId,
                deviceGroupName,
                device.LastSeenAt,
                device.CreatedAt);
    }
}
