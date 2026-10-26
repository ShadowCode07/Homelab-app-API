using HomelabAPI.Core.Domain;
using HomelabAPI.Presentation.DTOs.DeviceGroups;
using HomelabAPI.Presentation.DTOs.Devices;
using Riok.Mapperly.Abstractions;

namespace HomelabAPI.Presentation.Mapping
{
    [Mapper]
    public static partial class DeviceGroupMapper
    {
        [MapperIgnoreSource(nameof(DeviceGroup.CreatedAt))]
        public static partial DeviceGroupDto ToDto(this DeviceGroup group);

        public static IReadOnlyList<DeviceGroupDto> ToDtoList(this IEnumerable<DeviceGroup> groups)
            => groups.Select(g => g.ToDto()).ToList();
    }
}
