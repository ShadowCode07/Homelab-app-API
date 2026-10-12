using HomelabAPI.Core.Exceptions;

namespace HomelabAPI.Core.Domain
{
    public class DeviceGroup
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;

        public Guid Id { get; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; }

        public DeviceGroup(Guid id, string name, string? description, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
        }

        public static DeviceGroup Create(string name, string? description)
        {
            var cleanName = (name ?? string.Empty).Trim();
            if (cleanName.Length == 0)
                throw new DomainException(nameof(Name), "Group name is required.");
            if (cleanName.Length > NameMaxLength)
                throw new DomainException(nameof(Name), $"Group name can be at most {NameMaxLength} characters.");

            var cleanDescription = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            if (cleanDescription is not null && cleanDescription.Length > DescriptionMaxLength)
                throw new DomainException(nameof(Description), $"Description can be at most {DescriptionMaxLength} characters.");

            return new DeviceGroup(Guid.NewGuid(), cleanName, cleanDescription, DateTime.UtcNow);
        }
    }
}
