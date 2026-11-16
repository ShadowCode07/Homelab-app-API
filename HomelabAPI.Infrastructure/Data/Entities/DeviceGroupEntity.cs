using HomelabAPI.Core.Domain;

namespace HomelabAPI.Infrastructure.Data.Entities
{
    public class DeviceGroupEntity : BaseClass
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<DeviceEntity> Devices { get; set; } = new List<DeviceEntity>();

        // Entity -> domain
        public DeviceGroup ToDomain()
            => new DeviceGroup(Id, Name, Description, CreatedAt);

        // Domain -> entity
        public static DeviceGroupEntity FromDomain(DeviceGroup group)
            => new()
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                CreatedAt = group.CreatedAt
            };
    }
}
