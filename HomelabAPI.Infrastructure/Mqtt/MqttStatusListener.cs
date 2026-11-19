using HomelabAPI.Application.Interfaces.Services;
using HomelabAPI.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Protocol;

namespace HomelabAPI.Infrastructure.Mqtt;

public class MqttStatusListener : BackgroundService
{
    private readonly MqttOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MqttStatusListener> _logger;

    public MqttStatusListener(IOptions<MqttOptions> options, IServiceScopeFactory scopeFactory, ILogger<MqttStatusListener> logger)
    {
        _options = options.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new MqttClientFactory();
        using var client = factory.CreateMqttClient();
        
        var clientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(_options.Host, _options.Port)
            .WithClientId(_options.ClientId)
            .WithCleanSession()
            .Build();

        var subscribeOptions = factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(_options.StatusTopic, MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        client.ApplicationMessageReceivedAsync += e =>
            HandleMessageAsync(e.ApplicationMessage.Topic, e.ApplicationMessage.ConvertPayloadToString());

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!await client.TryPingAsync(stoppingToken))
                {
                    await client.ConnectAsync(clientOptions, stoppingToken);
                    await client.SubscribeAsync(subscribeOptions, stoppingToken);
                    _logger.LogInformation("MQTT connected to {Host}:{Port}, subscribed to {Topic}",
                        _options.Host, _options.Port, _options.StatusTopic);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning(ex, "MQTT broker unreachable, retrying in 5 s");
            }

            try { await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); }
            catch (OperationCanceledException) { break; }
        }
    }

    private async Task HandleMessageAsync(string topic, string payload)
    {
        var parts = topic.Split('/');

        if (parts.Length != 3 || !Guid.TryParse(parts[1], out var deviceId))
        {
            _logger.LogWarning("Ignoring MQTT message on {Topic}: no valid device id", topic);
            return;
        }

        DeviceStatus? status = payload.Trim().ToLowerInvariant() switch
        {
            "offline" => DeviceStatus.Offline,
            "online" => DeviceStatus.Online,
            _ => null
        };

        if (status == null)
        {
            _logger.LogWarning("Device with {DeviceId} - status unknows", deviceId);
            return;
        }

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IDeviceStatusService>();
            
            var found = await service.UpdateStatusAsync(deviceId, status.Value, DateTime.UtcNow);

            if (!found)
            {
                _logger.LogWarning("Device with {DeviceId} - status update failed", deviceId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Device with {DeviceId} - Error updating device status" , deviceId);
        }
    }
}