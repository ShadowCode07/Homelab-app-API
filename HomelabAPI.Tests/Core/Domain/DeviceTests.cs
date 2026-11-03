using HomelabAPI.Core.Domain;
using HomelabAPI.Core.Enums;
using HomelabAPI.Core.Exceptions;

namespace HomelabAPI.Tests.Core.Domain
{
    public class DeviceTests
    {
        private const string ValidName = "Kitchen Pi";
        private const string ValidHostname = "pi-kitchen";

        private static Device CreateDevice(
            string name = ValidName,
            string hostname = ValidHostname,
            DeviceType deviceType = DeviceType.Server,
            Guid? deviceGroupId = null)
            => new(name, hostname, deviceType, deviceGroupId);

        [Fact]
        public void Constructor_WithValidInput_SetsProperties()
        {
            var groupId = Guid.NewGuid();

            var device = CreateDevice(deviceType: DeviceType.Computer, deviceGroupId: groupId);

            Assert.Equal(ValidName, device.Name);
            Assert.Equal(ValidHostname, device.Hostname);
            Assert.Equal(DeviceType.Computer, device.DeviceType);
            Assert.Equal(groupId, device.DeviceGroupId);
        }

        [Fact]
        public void Constructor_NewDevice_HasDefaultState()
        {
            var before = DateTime.UtcNow;

            var device = CreateDevice();

            var after = DateTime.UtcNow;
            Assert.NotEqual(Guid.Empty, device.Id);
            Assert.Equal(DeviceStatus.Unknown, device.DeviceStatus);
            Assert.Null(device.IpAddress);
            Assert.Null(device.LastSeenAt);
            Assert.Null(device.DeviceGroup);
            Assert.InRange(device.CreatedAt, before, after);
        }

        [Fact]
        public void Constructor_TwoDevices_GetDifferentIds()
        {
            var first = CreateDevice();
            var second = CreateDevice();

            Assert.NotEqual(first.Id, second.Id);
        }

        [Fact]
        public void Constructor_TrimsName()
        {
            var device = CreateDevice(name: "   Kitchen Pi  ");

            Assert.Equal("Kitchen Pi", device.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_WithMissingName_Throws(string? name)
        {
            var ex = Assert.Throws<DomainException>(() => CreateDevice(name: name!));

            Assert.Equal(nameof(Device.Name), ex.Field);
        }

        [Fact]
        public void Constructor_WithNameAtMaxLength_Succeeds()
        {
            var name = new string('a', Device.NameMaxLength);

            var device = CreateDevice(name: name);

            Assert.Equal(name, device.Name);
        }

        [Fact]
        public void Constructor_WithNameOverMaxLength_Throws()
        {
            var name = new string('a', Device.NameMaxLength + 1);

            var ex = Assert.Throws<DomainException>(() => CreateDevice(name: name));

            Assert.Equal(nameof(Device.Name), ex.Field);
        }

        [Fact]
        public void Constructor_NameLengthIsCheckedAfterTrimming()
        {
            var name = "  " + new string('a', Device.NameMaxLength) + "  ";

            var device = CreateDevice(name: name);

            Assert.Equal(Device.NameMaxLength, device.Name.Length);
        }

        [Theory]
        [InlineData("NAS-01", "nas-01")]
        [InlineData("  pi-kitchen  ", "pi-kitchen")]
        [InlineData(" Nas.Home.LAN ", "nas.home.lan")]
        public void Constructor_NormalizesHostname(string input, string expected)
        {
            var device = CreateDevice(hostname: input);

            Assert.Equal(expected, device.Hostname);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_WithMissingHostname_Throws(string? hostname)
        {
            var ex = Assert.Throws<DomainException>(() => CreateDevice(hostname: hostname!));

            Assert.Equal(nameof(Device.Hostname), ex.Field);
            Assert.Equal("Hostname is required.", ex.Message);
        }

        [Theory]
        [InlineData("pi")]
        [InlineData("a")]
        [InlineData("123")]
        [InlineData("pi-kitchen")]
        [InlineData("nas.home.lan")]
        [InlineData("my--server")]
        public void Constructor_WithValidHostnameFormat_Succeeds(string hostname)
        {
            var device = CreateDevice(hostname: hostname);

            Assert.Equal(hostname, device.Hostname);
        }

        [Theory]
        [InlineData("-pi")]
        [InlineData("pi-")]
        [InlineData("nas.-home")]
        [InlineData("a..b")]
        [InlineData("nas.")]
        [InlineData(".nas")]
        [InlineData("pi_kitchen")]
        [InlineData("has space")]
        [InlineData("pi@home")]
        public void Constructor_WithInvalidHostnameFormat_Throws(string hostname)
        {
            var ex = Assert.Throws<DomainException>(() => CreateDevice(hostname: hostname));

            Assert.Equal(nameof(Device.Hostname), ex.Field);
            Assert.Equal("Invalid hostname format.", ex.Message);
        }

        [Fact]
        public void Constructor_WithLabelOf63Characters_Succeeds()
        {
            var hostname = new string('a', 63);

            var device = CreateDevice(hostname: hostname);

            Assert.Equal(hostname, device.Hostname);
        }

        [Fact]
        public void Constructor_WithLabelOf64Characters_Throws()
        {
            var hostname = new string('a', 64);

            var ex = Assert.Throws<DomainException>(() => CreateDevice(hostname: hostname));

            Assert.Equal("Invalid hostname format.", ex.Message);
        }

        [Fact]
        public void Constructor_WithHostnameAtMaxLength_Succeeds()
        {
            var hostname = BuildHostname(Device.HostnameMaxLength);

            var device = CreateDevice(hostname: hostname);

            Assert.Equal(Device.HostnameMaxLength, device.Hostname.Length);
        }

        [Fact]
        public void Constructor_WithHostnameOverMaxLength_Throws()
        {
            var hostname = BuildHostname(Device.HostnameMaxLength + 1);

            var ex = Assert.Throws<DomainException>(() => CreateDevice(hostname: hostname));

            Assert.Equal(nameof(Device.Hostname), ex.Field);
            Assert.Contains("at most", ex.Message);
        }

        [Fact]
        public void AssignGroup_WithMatchingGroup_SetsGroup()
        {
            var group = new DeviceGroup("Server rack", null);
            var device = CreateDevice(deviceGroupId: group.Id);

            device.AssignGroup(group);

            Assert.Same(group, device.DeviceGroup);
        }

        [Fact]
        public void AssignGroup_WithDifferentGroup_Throws()
        {
            var group = new DeviceGroup("Server rack", null);
            var device = CreateDevice(deviceGroupId: Guid.NewGuid());

            var ex = Assert.Throws<DomainException>(() => device.AssignGroup(group));

            Assert.Equal(nameof(Device.DeviceGroupId), ex.Field);
            Assert.Null(device.DeviceGroup);
        }

        [Fact]
        public void AssignGroup_WhenDeviceHasNoGroupId_Throws()
        {
            var group = new DeviceGroup("Server rack", null);
            var device = CreateDevice(deviceGroupId: null);

            Assert.Throws<DomainException>(() => device.AssignGroup(group));
        }

        private static string BuildHostname(int length)
        {
            var label = new string('a', 63);
            var longHostname = string.Join('.', label, label, label, label);
            return longHostname[..length];
        }
    }
}
