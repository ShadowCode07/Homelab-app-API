using HomelabAPI.Core.Enums;

namespace HomelabAPI.Presentation.DTOs.Devices
{
    public record DeviceDto
    (
        Guid Id,
        string Name,
        string Hostname,
        DeviceType DeviceType,
        DeviceStatus DeviceStatus,
        string? IpAddress,
        Guid? DeviceGroupId,
        string? DeviceGroupName,
        DateTime? LastSeenAt,
        DateTime CreatedAt
    );
}
