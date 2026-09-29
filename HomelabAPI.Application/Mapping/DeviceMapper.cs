using HomelabAPI.Application.DTOs.Devices;
using HomelabAPI.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace HomelabAPI.Application.Mapping
{
    [Mapper]
    public static partial class DeviceMapper
    {
        [MapProperty(new[] { nameof(Device.DeviceGroup), nameof(DeviceGroup.Name) }, nameof(DeviceDto.DeviceGroupName))]
        public static partial DeviceDto ToDto(this Device device);

        public static partial IReadOnlyList<DeviceDto> ToDtoList(this IEnumerable<Device> devices);

        [MapperIgnoreTarget(nameof(Device.Id))]
        [MapperIgnoreTarget(nameof(Device.DeviceStatus))]
        [MapperIgnoreTarget(nameof(Device.LastSeenAt))]
        [MapperIgnoreTarget(nameof(Device.CreatedAt))]
        [MapperIgnoreTarget(nameof(Device.DeviceGroup))]
        public static partial Device ToEntity(this DeviceCreateDto dto);
    }
}
