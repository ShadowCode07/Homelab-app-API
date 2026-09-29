using HomelabAPI.Core.Enums;

namespace HomelabAPI.Infrastructure.Data
{
    public static class DeviceSeed
    {
        private static readonly DateTime SeedDate = new(2026, 9, 29, 0, 0, 0, DateTimeKind.Utc);

        public static object[] Devices =>
        [
            new { Id = Guid.Parse("3b9a7c10-5d2e-4f81-a6c4-000000000001"), Name = "Proxmox host", Hostname = "pve-01", DeviceType = DeviceType.Server, DeviceStatus = DeviceStatus.Unknown, IpAddress = "192.168.1.10", DeviceGroupId = (Guid?)DeviceGroupSeed.ServerRackId, CreatedAt = SeedDate },
            new { Id = Guid.Parse("3b9a7c10-5d2e-4f81-a6c4-000000000002"), Name = "NAS", Hostname = "nas-01", DeviceType = DeviceType.Server, DeviceStatus = DeviceStatus.Unknown, IpAddress = "192.168.1.20", DeviceGroupId = (Guid?)DeviceGroupSeed.ServerRackId, CreatedAt = SeedDate },
            new { Id = Guid.Parse("3b9a7c10-5d2e-4f81-a6c4-000000000003"), Name = "Main desktop", Hostname = "desktop-main", DeviceType = DeviceType.Computer, DeviceStatus = DeviceStatus.Unknown, IpAddress = "192.168.1.50", DeviceGroupId = (Guid?)DeviceGroupSeed.OfficeId, CreatedAt = SeedDate },
            new { Id = Guid.Parse("3b9a7c10-5d2e-4f81-a6c4-000000000004"), Name = "Living room Pi", Hostname = "pi-livingroom", DeviceType = DeviceType.Computer, DeviceStatus = DeviceStatus.Unknown, IpAddress = "192.168.1.60", DeviceGroupId = (Guid?)DeviceGroupSeed.LivingRoomId, CreatedAt = SeedDate },
            new { Id = Guid.Parse("3b9a7c10-5d2e-4f81-a6c4-000000000005"), Name = "Phone", Hostname = "phone-01", DeviceType = DeviceType.Mobile, DeviceStatus = DeviceStatus.Unknown, CreatedAt = SeedDate },
        ];
    }
}
