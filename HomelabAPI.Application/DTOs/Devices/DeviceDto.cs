using HomelabAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Application.DTOs.Devices
{
    public record DeviceDto
    (
        Guid Id,
        string Name,
        DeviceType DeviceType,
        DeviceStatus DeviceStatus,
        string? IpAddress,
        DateTime? LastSeenAt,
        DateTime CreatedAt
    );
}
