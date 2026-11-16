using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomelabAPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceHostnameAndGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Devices",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Devices",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hostname",
                table: "Devices",
                type: "nvarchar(253)",
                maxLength: 253,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                "UPDATE [Devices] SET [Hostname] = LOWER(CONVERT(nvarchar(36), [Id])) WHERE [Hostname] = N'';");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceGroupId",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeviceGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceGroups", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DeviceGroups",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e01"), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Rack-mounted servers and network gear", "Server rack" },
                    { new Guid("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e02"), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Desk computers and peripherals", "Office" },
                    { new Guid("8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e03"), new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Media and smart home devices", "Living room" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceGroupId",
                table: "Devices",
                column: "DeviceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Hostname",
                table: "Devices",
                column: "Hostname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceGroups_Name",
                table: "DeviceGroups",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_DeviceGroups_DeviceGroupId",
                table: "Devices",
                column: "DeviceGroupId",
                principalTable: "DeviceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_DeviceGroups_DeviceGroupId",
                table: "Devices");

            migrationBuilder.DropTable(
                name: "DeviceGroups");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceGroupId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_Hostname",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "DeviceGroupId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Hostname",
                table: "Devices");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45,
                oldNullable: true);
        }
    }
}
