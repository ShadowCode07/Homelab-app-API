using HomelabAPI.Presentation.Validation;
using HomelabAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomelabAPI.Presentation.DTOs.Devices
{
    public record DeviceCreateDto
    {
        [Required(ErrorMessage = "Device name is required.")]
        [StringLength(100, ErrorMessage = "Device name can be at most 100 characters.")]
        public required string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "Hostname is required.")]
        [Hostname]
        public required string Hostname { get; init; } = string.Empty;

        [EnumDataType(typeof(DeviceType), ErrorMessage = "Device type is not a valid value.")]
        public DeviceType DeviceType { get; init; } = DeviceType.Other;
             
        public Guid? DeviceGroupId { get; init; }
    }
}
