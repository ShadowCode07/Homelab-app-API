using HomelabAPI.Core.Domain;
using HomelabAPI.Core.Exceptions;

namespace HomelabAPI.Tests.Core.Domain
{
    public class DeviceGroupTests
    {
        [Fact]
        public void Constructor_WithValidInput_SetsProperties()
        {
            var before = DateTime.UtcNow;

            var group = new DeviceGroup("Server rack", "Everything in the closet");

            var after = DateTime.UtcNow;
            Assert.NotEqual(Guid.Empty, group.Id);
            Assert.Equal("Server rack", group.Name);
            Assert.Equal("Everything in the closet", group.Description);
            Assert.InRange(group.CreatedAt, before, after);
        }

        [Fact]
        public void Constructor_TrimsName()
        {
            var group = new DeviceGroup("  Office  ", null);

            Assert.Equal("Office", group.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_WithMissingName_Throws(string? name)
        {
            var ex = Assert.Throws<DomainException>(() => new DeviceGroup(name!, null));

            Assert.Equal(nameof(DeviceGroup.Name), ex.Field);
        }

        [Fact]
        public void Constructor_WithNameAtMaxLength_Succeeds()
        {
            var name = new string('a', DeviceGroup.NameMaxLength);

            var group = new DeviceGroup(name, null);

            Assert.Equal(name, group.Name);
        }

        [Fact]
        public void Constructor_WithNameOverMaxLength_Throws()
        {
            var name = new string('a', DeviceGroup.NameMaxLength + 1);

            var ex = Assert.Throws<DomainException>(() => new DeviceGroup(name, null));

            Assert.Equal(nameof(DeviceGroup.Name), ex.Field);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Constructor_WithEmptyDescription_StoresNull(string? description)
        {
            var group = new DeviceGroup("Office", description);

            Assert.Null(group.Description);
        }

        [Fact]
        public void Constructor_TrimsDescription()
        {
            var group = new DeviceGroup("Office", "  Desk and printer  ");

            Assert.Equal("Desk and printer", group.Description);
        }

        [Fact]
        public void Constructor_WithDescriptionAtMaxLength_Succeeds()
        {
            var description = new string('d', DeviceGroup.DescriptionMaxLength);

            var group = new DeviceGroup("Office", description);

            Assert.Equal(description, group.Description);
        }

        [Fact]
        public void Constructor_WithDescriptionOverMaxLength_Throws()
        {
            var description = new string('d', DeviceGroup.DescriptionMaxLength + 1);

            var ex = Assert.Throws<DomainException>(() => new DeviceGroup("Office", description));

            Assert.Equal(nameof(DeviceGroup.Description), ex.Field);
        }
    }
}
