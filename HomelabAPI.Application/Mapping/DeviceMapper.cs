using HomelabAPI.Application.DTOs.Devices;
using HomelabAPI.Core.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Application.Mapping
{
    [Mapper]
    public static partial class DeviceMapper
    {
        public static partial DeviceDto ToDto(this Device device);
        public static partial IReadOnlyList<DeviceDto> ToDtoList(this IEnumerable<Device> devices);

        [MapperIgnoreTarget(nameof(Device.Id))]
        [MapperIgnoreTarget(nameof(Device.DeviceStatus))]
        [MapperIgnoreTarget(nameof(Device.LastSeenAt))]
        [MapperIgnoreTarget(nameof(Device.CreatedAt))]
        public static partial Device ToEntity(this DeviceCreateDto dto);
    }
}
