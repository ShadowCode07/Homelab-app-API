using HomelabAPI.Core.Domain;
using HomelabAPI.Core.Exceptions;

namespace HomelabAPI.Application.Exceptions
{
    public class DuplicateHostnameException : AppException
    {
        public string Hostname { get; }

        public DuplicateHostnameException(string hostname)
            : base($"A device with hostname {hostname} is already registered",
                  ErrorCategory.Conflict,
                  ErrorCodes.DuplicateHostname,
                  field: nameof(Device.Hostname),
                  title: "Hostname is already in use.")
        {
            Hostname = hostname;
        }
    }
}
