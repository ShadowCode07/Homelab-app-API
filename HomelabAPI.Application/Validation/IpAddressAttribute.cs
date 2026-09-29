using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

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

            ip = ip.Trim();
            if (!IPAddress.TryParse(ip, out var parsed))
                return false;

            return parsed.AddressFamily switch
            {
                AddressFamily.InterNetwork => ip.Split('.').Length == 4,
                AddressFamily.InterNetworkV6 => true,
                _ => false
            };
        }
    }
}
