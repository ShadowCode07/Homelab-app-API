using HomelabAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomelabAPI.Tests.Integration
{
    // Boots the REAL API (Program.cs, middleware, controllers, handler) in memory,
    // but swaps SQL Server for an in-memory database so tests need no real DB.
    public class HomelabApiFactory : WebApplicationFactory<Program>
    {
        // Unique per factory, so test classes never share data.
        private readonly string _databaseName = $"homelab-tests-{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Not "Development": no developer exception page, so we test what production would return.
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove the SQL Server options registered in Program.cs...
                var sqlServerOptions = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (sqlServerOptions != null)
                    services.Remove(sqlServerOptions);

                // ...and register an in-memory database instead.
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(_databaseName));
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            // EnsureCreated applies the HasData seed (pve-01, nas-01, ... and the 3 groups).
            using var scope = host.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();

            return host;
        }
    }
}
