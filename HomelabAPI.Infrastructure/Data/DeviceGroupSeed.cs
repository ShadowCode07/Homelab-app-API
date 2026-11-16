namespace HomelabAPI.Infrastructure.Data
{
    public static class DeviceGroupSeed
    {
        private static readonly DateTime SeedDate = new(2026, 9, 29, 0, 0, 0, DateTimeKind.Utc);

        public static readonly Guid ServerRackId = Guid.Parse("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e01");
        public static readonly Guid OfficeId = Guid.Parse("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e02");
        public static readonly Guid LivingRoomId = Guid.Parse("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e03");

        public static object[] Groups =>
        [
            new { Id = ServerRackId, Name = "Server rack", Description = "Rack-mounted servers and network gear", CreatedAt = SeedDate },
            new { Id = OfficeId, Name = "Office", Description = "Desk computers and peripherals", CreatedAt = SeedDate },
            new { Id = LivingRoomId, Name = "Living room", Description = "Media and smart home devices", CreatedAt = SeedDate },
        ];
    }
}
