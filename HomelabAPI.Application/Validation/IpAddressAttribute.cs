using HomelabAPI.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace HomelabAPI.Application.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class IpAddressAttribute : ValidationAttribute
    {
        public IpAddressAttribute()
            : base("IP address must be a valid IPv4 (e.g. 192.168.1.10) or IPv6 address.")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value is not string ip || string.IsNullOrWhiteSpace(ip))
                return true;

            return Device.IsValidIpAddress(ip);
        }
    }
}
