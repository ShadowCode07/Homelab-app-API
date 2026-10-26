using HomelabAPI.Core.Enums;
using HomelabAPI.Core.Exceptions;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace HomelabAPI.Core.Domain
{
    public class Device
    {
        public const int NameMaxLength = 100;
        public const int HostnameMaxLength = 253;
        public const int IpAddressMaxLength = 45;
        private static readonly Regex HostnameRegex = new(
            @"^(?!-)[A-Za-z0-9-]{1,63}(?<!-)(\.(?!-)[A-Za-z0-9-]{1,63}(?<!-))*$",
            RegexOptions.Compiled);

        public Guid Id { get; }
        public string Name { get; private set; }
        public string Hostname { get; }
        public DeviceType DeviceType { get; private set; }
        public DeviceStatus DeviceStatus { get; private set; }
        public string? IpAddress { get; private set; }
        public DateTime? LastSeenAt { get; private set; }
        public DateTime CreatedAt { get; }
        public Guid? DeviceGroupId { get; private set; }
        public DeviceGroup? DeviceGroup { get; private set; }

        public Device(
            Guid id,
            string name,
            string hostname,
            DeviceType deviceType,
            DeviceStatus deviceStatus,
            string? ipAddress,
            DateTime? lastSeenAt,
            DateTime createdAt,
            Guid? deviceGroupId,
            DeviceGroup? deviceGroup = null)
        {
            Id = id;
            Name = name;
            Hostname = hostname;
            DeviceType = deviceType;
            DeviceStatus = deviceStatus;
            IpAddress = ipAddress;
            LastSeenAt = lastSeenAt;
            CreatedAt = createdAt;
            DeviceGroupId = deviceGroupId;
            DeviceGroup = deviceGroup;
        }

        public static Device Create(string name, string hostname, DeviceType deviceType, Guid? deviceGroupId)
        {
            var cleanName = (name ?? string.Empty).Trim();
            if (cleanName.Length == 0)
                throw new DomainException(nameof(Name), "Device name is required.");
            if (cleanName.Length > NameMaxLength)
                throw new DomainException(nameof(Name), $"Device name can be at most {NameMaxLength} characters.");

            var cleanHostname = NormalizeHostname(hostname);
            if (cleanHostname.Length == 0)
                throw new DomainException(nameof(Hostname), "Hostname is required.");
            if (cleanHostname.Length > HostnameMaxLength)
                throw new DomainException(nameof(Hostname), $"Hostname can be at most {HostnameMaxLength} characters.");
            if (!IsValidHostname(cleanHostname))
                throw new DomainException(nameof(Hostname), "Hostname may only contain letters, digits, hyphens and dots, and each part must start and end with a letter or digit.");

            return new Device(
                Guid.NewGuid(),
                cleanName,
                cleanHostname,
                deviceType,
                DeviceStatus.Unknown,
                ipAddress: null,
                lastSeenAt: null,
                createdAt: DateTime.UtcNow,
                deviceGroupId);
        }

        public static string NormalizeHostname(string hostname)
            => (hostname ?? string.Empty).Trim().ToLowerInvariant();

        public static bool IsValidHostname(string hostname)
        {
            var trimmed = hostname.Trim();
            return trimmed.Length <= HostnameMaxLength && HostnameRegex.IsMatch(trimmed);
        }

        public static bool IsValidIpAddress(string ipAddress)
        {
            var trimmed = ipAddress.Trim();
            if (!IPAddress.TryParse(trimmed, out var parsed))
                return false;

            return parsed.AddressFamily switch
            {
                AddressFamily.InterNetwork => trimmed.Split('.').Length == 4,
                AddressFamily.InterNetworkV6 => true,
                _ => false
            };
        }

        public void AssignGroup(DeviceGroup deviceGroup)
        {
            if (deviceGroup.Id != DeviceGroupId)
                throw new DomainException(nameof(DeviceGroupId), "Groups don't match");

            DeviceGroup = deviceGroup;
        }
    }
}
