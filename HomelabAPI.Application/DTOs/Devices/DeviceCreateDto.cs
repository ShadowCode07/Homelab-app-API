using HomelabAPI.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Application.DTOs.Devices
{
    public record DeviceCreateDto
    (
        [Required, MaxLength(100)] string Name,
        DeviceType DeviceType,
        string? IpAddress
    );
    
}
