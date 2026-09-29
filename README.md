# Homelab manager

ASP.NET Core 8 API for registering, monitoring and managing homelab devices.

## Projects

| Project | Responsibility |
| --- | --- |
| `HomelabAPI.Core` | Domain entities and enums (`Device`, `DeviceGroup`, `DeviceType`, `DeviceStatus`) |
| `HomelabAPI.Application` | DTOs, validation, services (use cases), repository interfaces, Mapperly mappers |
| `HomelabAPI.Infrastructure` | EF Core `ApplicationDbContext`, migrations, repository implementations |
| `HomelabAPI.Presentation` | REST controllers, DI setup, Swagger |

## Running locally

1. Put the SQL Server connection string in `.env` (next to the `.sln`):
   `ConnectionStrings__DefaultConnection=Server=...;Database=Homelab;...`
2. Apply migrations:
   `dotnet ef database update --project HomelabAPI.Infrastructure --startup-project HomelabAPI.Presentation`
3. Run the API: `dotnet run --project HomelabAPI.Presentation` and open `/swagger`.

## Device registration API

| Method | Route | Result |
| --- | --- | --- |
| `GET` | `/api/device` | All devices (the inventory), sorted by name |
| `GET` | `/api/device/{id}` | One device, `404` if it does not exist |
| `POST` | `/api/device` | Register a device: `201` + `Location` header, `400` on invalid input, `409` if the hostname is taken |
| `GET` | `/api/devicegroup` | Groups/locations a device can be assigned to |

Registration body:

```json
{
  "name": "Kitchen Pi",
  "hostname": "pi-kitchen",
  "deviceType": "Computer",
  "ipAddress": "192.168.1.42",
  "deviceGroupId": "8f5d2a4e-3c1b-4e6a-9b7d-1a2b3c4d5e03"
}
```

Validation rules:

- `name` – required, max 100 characters.
- `hostname` – required, a valid RFC 1123 hostname (e.g. `pi-kitchen`, `nas.home.lan`). It is the device's unique identifier: stored in lower case and compared case-insensitively, so `PI-Kitchen` and `pi-kitchen` are the same device.
- `deviceType` – optional, one of `Other` (default), `Server`, `Computer`, `Mobile`.
- `ipAddress` – optional, a full IPv4 (`192.168.1.42`) or IPv6 address.
- `deviceGroupId` – optional, must be an existing group.

Errors use the standard `ValidationProblemDetails` shape (`errors: { "Hostname": ["..."] }`) for both `400` and `409`, so a client can show each message next to its field.
