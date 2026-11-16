using HomelabAPI.Core.Domain;
using HomelabAPI.Core.Enums;

namespace HomelabAPI.Infrastructure.Data.Entities
{
    public class DeviceEntity : BaseClass
    {
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public DeviceType DeviceType { get; set; }
        public DeviceStatus DeviceStatus { get; set; }
        public string? IpAddress { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? DeviceGroupId { get; set; }
        public DeviceGroupEntity? DeviceGroup { get; set; }

        // Entity -> domain
        public Device ToDomain()
            => new Device(
                Id,
                Name,
                Hostname,
                DeviceType,
                DeviceStatus,
                IpAddress,
                LastSeenAt,
                CreatedAt,
                DeviceGroupId,
                DeviceGroup?.ToDomain());

        // Domain -> entity
        public static DeviceEntity FromDomain(Device device)
            => new()
            {
                Id = device.Id,
                Name = device.Name,
                Hostname = device.Hostname,
                DeviceType = device.DeviceType,
                DeviceStatus = device.DeviceStatus,
                IpAddress = device.IpAddress,
                LastSeenAt = device.LastSeenAt,
                CreatedAt = device.CreatedAt,
                DeviceGroupId = device.DeviceGroupId
                // Only the FK on purpose: setting the DeviceGroup navigation here would make
                // EF track it as a new entity and try to INSERT the existing group again.
            };
    }
}
