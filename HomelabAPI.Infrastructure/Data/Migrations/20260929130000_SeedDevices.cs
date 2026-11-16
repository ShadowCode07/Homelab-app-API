using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomelabAPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDevices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "CreatedAt", "DeviceGroupId", "DeviceStatus", "DeviceType", "Hostname", "IpAddress", "LastSeenAt", "Name" },
                values: new object[,]
                {
                    { new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000001"), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e01"), 0, 1, "pve-01", "192.168.1.10", null, "Proxmox host" },
                    { new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000002"), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e01"), 0, 1, "nas-01", "192.168.1.20", null, "NAS" },
                    { new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000003"), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e02"), 0, 2, "desktop-main", "192.168.1.50", null, "Main desktop" },
                    { new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000004"), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e03"), 0, 2, "pi-livingroom", "192.168.1.60", null, "Living room Pi" },
                    { new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000005"), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, 0, 3, "phone-01", null, null, "Phone" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000001"));

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000002"));

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000003"));

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000004"));

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("3b9a7c10-5d2e-4f81-a6c4-000000000005"));
        }
    }
}
