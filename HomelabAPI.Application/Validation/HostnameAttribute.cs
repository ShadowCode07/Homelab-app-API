using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HomelabAPI.Application.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed partial class HostnameAttribute : ValidationAttribute
    {
        public const int MaxLength = 253;

        public HostnameAttribute()
            : base("Hostname may only contain letters, digits, hyphens and dots, and each part must start and end with a letter or digit (e.g. 'pi-kitchen' or 'nas.home.lan').")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value is not string hostname || string.IsNullOrWhiteSpace(hostname))
                return true;

            hostname = hostname.Trim();
            return hostname.Length <= MaxLength && HostnameRegex().IsMatch(hostname);
        }

        [GeneratedRegex(@"^(?!-)[A-Za-z0-9-]{1,63}(?<!-)(\.(?!-)[A-Za-z0-9-]{1,63}(?<!-))*$")]
        private static partial Regex HostnameRegex();
    }
}
