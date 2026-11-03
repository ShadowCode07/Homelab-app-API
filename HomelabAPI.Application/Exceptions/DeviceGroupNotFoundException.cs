using HomelabAPI.Core.Domain;
using HomelabAPI.Core.Exceptions;

namespace HomelabAPI.Application.Exceptions
{
    public class DeviceGroupNotFoundException : AppException
    {
        public Guid DeviceGroupId { get; }

        public DeviceGroupNotFoundException(Guid deviceGroupId)
            : base($"Device group with id {deviceGroupId} not found",
                  ErrorCategory.Validation,
                  ErrorCodes.DeviceGroupNotFound,
                  field: nameof(Device.DeviceGroupId),
                  title: "Device group does not exist.")
        {
            DeviceGroupId = deviceGroupId;
        }
    }
}
