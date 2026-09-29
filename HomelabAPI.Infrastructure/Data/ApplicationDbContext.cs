using HomelabAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomelabAPI.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceGroup> DeviceGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Device>(device =>
            {
                device.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                device.Property(d => d.Hostname)
                    .IsRequired()
                    .HasMaxLength(253);

                device.HasIndex(d => d.Hostname)
                    .IsUnique();

                device.Property(d => d.IpAddress)
                    .HasMaxLength(45);

                device.HasOne(d => d.DeviceGroup)
                    .WithMany(g => g.Devices)
                    .HasForeignKey(d => d.DeviceGroupId)
                    .OnDelete(DeleteBehavior.SetNull);

                device.HasData(DeviceSeed.Devices);
            });

            modelBuilder.Entity<DeviceGroup>(group =>
            {
                group.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                group.HasIndex(g => g.Name)
                    .IsUnique();

                group.Property(g => g.Description)
                    .HasMaxLength(500);

                group.HasData(DeviceGroupSeed.Groups);
            });
        }
    }
}
