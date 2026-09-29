namespace HomelabAPI.Core.Entities
{
    public class DeviceGroup : BaseClass
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Device> Devices { get; set; } = new List<Device>();
    }
}
