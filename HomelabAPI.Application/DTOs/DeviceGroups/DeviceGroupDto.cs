namespace HomelabAPI.Application.DTOs.DeviceGroups
{
    public record DeviceGroupDto
    (
        Guid Id,
        string Name,
        string? Description
    );
}
