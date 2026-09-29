using HomelabAPI.Application.Validation;
using HomelabAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomelabAPI.Application.DTOs.Devices
{
    public record DeviceCreateDto
    {
        [Required(ErrorMessage = "Device name is required.")]
        [StringLength(100, ErrorMessage = "Device name can be at most 100 characters.")]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "Hostname is required.")]
        [Hostname]
        public string Hostname { get; init; } = string.Empty;

        [EnumDataType(typeof(DeviceType), ErrorMessage = "Device type is not a valid value.")]
        public DeviceType DeviceType { get; init; } = DeviceType.Other;

        [IpAddress]
        public string? IpAddress { get; init; }

        public Guid? DeviceGroupId { get; init; }
    }
}
