using HomelabAPI.Presentation.DTOs.Devices;
using HomelabAPI.Core.Domain;

namespace HomelabAPI.Presentation.Mapping
{
    public static class DeviceMapper
    {
        public static DeviceDto ToDto(this Device device)
            => new(
                device.Id,
                device.Name,
                device.Hostname,
                device.DeviceType,
                device.DeviceStatus,
                device.IpAddress,
                device.DeviceGroupId,
                device.DeviceGroup?.Name,
                device.LastSeenAt,
                device.CreatedAt);

        public static IReadOnlyList<DeviceDto> ToDtoList(this IEnumerable<Device> devices)
            => devices.Select(d => d.ToDto()).ToList();

        public static Device ToDomain(this DeviceCreateDto dto)
            => Device.Create(dto.Name, dto.Hostname, dto.DeviceType, dto.DeviceGroupId);
    }
}
