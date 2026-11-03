using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Core.Domain;
using HomelabAPI.Core.Exceptions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace HomelabAPI.Tests.Integration
{
    public class DeviceEndpointsTests : IClassFixture<HomelabApiFactory>
    {
        private const string DevicesUrl = "/api/device";

        private readonly HomelabApiFactory _factory;
        private readonly HttpClient _client;

        public DeviceEndpointsTests(HomelabApiFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        // Unique per call so tests that create devices never collide with each other or the seed.
        private static string UniqueHostname() => $"test-{Guid.NewGuid():N}";

        private static async Task<JsonElement> ReadJsonAsync(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(body).RootElement;
        }

        [Fact]
        public async Task CreateDevice_WithValidInput_Returns201()
        {
            var response = await _client.PostAsJsonAsync(DevicesUrl, new
            {
                name = "Kitchen Pi",
                hostname = UniqueHostname(),
                deviceType = "Computer"
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

        [Theory]
        [InlineData("nas-01")]
        [InlineData("NAS-01")] // the domain lower-cases hostnames, so this must collide too
        public async Task CreateDevice_WithExistingHostname_Returns409FromHandler(string hostname)
        {
            var response = await _client.PostAsJsonAsync(DevicesUrl, new
            {
                name = "Second NAS",
                hostname,
                deviceType = "Server"
            });

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

            var problem = await ReadJsonAsync(response);
            Assert.Equal(409, problem.GetProperty("status").GetInt32());
            Assert.Equal("Hostname is already in use.", problem.GetProperty("title").GetString());
            Assert.Equal(ErrorCodes.DuplicateHostname, problem.GetProperty("errorCode").GetString());

            // The field-level shape the register form depends on: errors.Hostname[0]
            var hostnameErrors = problem.GetProperty("errors").GetProperty(nameof(Device.Hostname));
            Assert.Contains("nas-01", hostnameErrors[0].GetString());
        }

        [Fact]
        public async Task CreateDevice_WithUnknownGroup_Returns400FromHandler()
        {
            var response = await _client.PostAsJsonAsync(DevicesUrl, new
            {
                name = "Orphan device",
                hostname = UniqueHostname(),
                deviceType = "Other",
                deviceGroupId = Guid.NewGuid()
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var problem = await ReadJsonAsync(response);
            Assert.Equal("Device group does not exist.", problem.GetProperty("title").GetString());
            Assert.Equal(ErrorCodes.DeviceGroupNotFound, problem.GetProperty("errorCode").GetString());
            Assert.True(problem.GetProperty("errors").TryGetProperty(nameof(Device.DeviceGroupId), out _));
        }

        [Fact]
        public async Task CreateDevice_WithInvalidHostname_Returns400FromModelValidation()
        {
            // Caught by [Hostname] + [ApiController] BEFORE the controller runs -> no exception thrown.
            var response = await _client.PostAsJsonAsync(DevicesUrl, new
            {
                name = "Bad host",
                hostname = "-bad-",
                deviceType = "Other"
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var problem = await ReadJsonAsync(response);
            Assert.True(problem.GetProperty("errors").TryGetProperty(nameof(Device.Hostname), out _));

            // No errorCode = this response came from the framework's validation, not from our handler.
            Assert.False(problem.TryGetProperty("errorCode", out _));
        }

        [Fact]
        public async Task UnexpectedException_Returns500WithoutLeakingDetails()
        {
            // Same API, but with a service that crashes like a broken database would.
            var client = _factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
                    services.AddScoped<IDeviceService, ExplodingDeviceService>()))
                .CreateClient();

            var response = await client.GetAsync(DevicesUrl);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.DoesNotContain(ExplodingDeviceService.SecretDetail, body);

            var problem = JsonDocument.Parse(body).RootElement;
            Assert.Equal("An unexpected error occurred.", problem.GetProperty("title").GetString());
            Assert.False(problem.TryGetProperty("errorCode", out _));
        }

        // Simulates an infrastructure failure: NOT an AppException.
        private sealed class ExplodingDeviceService : IDeviceService
        {
            public const string SecretDetail = "Server=prod-sql;Password=hunter2";

            public Task<IReadOnlyList<Device>> GetDevicesAsync()
                => throw new InvalidOperationException($"Connection failed: {SecretDetail}");

            public Task<Device?> GetDeviceByIdAsync(Guid id)
                => throw new InvalidOperationException(SecretDetail);

            public Task<Device> CreateDeviceAsync(Device device)
                => throw new InvalidOperationException(SecretDetail);
        }
    }
}
