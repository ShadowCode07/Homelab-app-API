using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Core.Exceptions
{
    public static class ErrorCodes
    {
        public const string DomainValidation = "DOMAIN_VALIDATION_ERROR";
        public const string DuplicateHostname = "DUPLICATE_HOSTNAME";
        public const string DeviceGroupNotFound = "DEVICE_GROUP_NOT_FOUND";
    }
}
