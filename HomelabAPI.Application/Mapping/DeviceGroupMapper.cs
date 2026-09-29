using HomelabAPI.Application.DTOs.DeviceGroups;
using HomelabAPI.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace HomelabAPI.Application.Mapping
{
    [Mapper]
    public static partial class DeviceGroupMapper
    {
        [MapperIgnoreSource(nameof(DeviceGroup.Devices))]
        [MapperIgnoreSource(nameof(DeviceGroup.CreatedAt))]
        public static partial DeviceGroupDto ToDto(this DeviceGroup group);

        public static partial IReadOnlyList<DeviceGroupDto> ToDtoList(this IEnumerable<DeviceGroup> groups);
    }
}
