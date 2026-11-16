using HomelabAPI.Core.Domain;
using HomelabAPI.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomelabAPI.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<DeviceGroupEntity> DeviceGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeviceEntity>(device =>
            {
                device.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(Device.NameMaxLength);

                device.Property(d => d.Hostname)
                    .IsRequired()
                    .HasMaxLength(Device.HostnameMaxLength);

                device.HasIndex(d => d.Hostname)
                    .IsUnique();

                device.Property(d => d.IpAddress)
                    .HasMaxLength(Device.IpAddressMaxLength);

                device.HasOne(d => d.DeviceGroup)
                    .WithMany(g => g.Devices)
                    .HasForeignKey(d => d.DeviceGroupId)
                    .OnDelete(DeleteBehavior.SetNull);

                device.HasData(DeviceSeed.Devices);
            });

            modelBuilder.Entity<DeviceGroupEntity>(group =>
            {
                group.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(DeviceGroup.NameMaxLength);

                group.HasIndex(g => g.Name)
                    .IsUnique();

                group.Property(g => g.Description)
                    .HasMaxLength(DeviceGroup.DescriptionMaxLength);

                group.HasData(DeviceGroupSeed.Groups);
            });
        }
    }
}
