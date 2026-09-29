using HomelabAPI.Core.Enums;

namespace HomelabAPI.Core.Entities
{
    public class Device : BaseClass
    {
        public string Name { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; } = DeviceType.Other;
        public DeviceStatus DeviceStatus { get; set; } = DeviceStatus.Unknown;
        public string? IpAddress { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
