using HomelabAPI.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace HomelabAPI.Application.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class HostnameAttribute : ValidationAttribute
    {
        public HostnameAttribute()
            : base("Hostname may only contain letters, digits, hyphens and dots, and each part must start and end with a letter or digit (e.g. 'pi-kitchen' or 'nas.home.lan').")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value is not string hostname || string.IsNullOrWhiteSpace(hostname))
                return true;

            return Device.IsValidHostname(hostname);
        }
    }
}
