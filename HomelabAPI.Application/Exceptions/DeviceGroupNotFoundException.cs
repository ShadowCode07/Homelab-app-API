namespace HomelabAPI.Application.Exceptions
{
    public class DeviceGroupNotFoundException : Exception
    {
        public Guid DeviceGroupId { get; }

        public DeviceGroupNotFoundException(Guid deviceGroupId)
            : base($"Device group '{deviceGroupId}' does not exist.")
        {
            DeviceGroupId = deviceGroupId;
        }
    }
}
