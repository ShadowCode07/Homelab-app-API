namespace HomelabAPI.Infrastructure.Mqtt;

public class MqttOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 1883;
    public string ClientId { get; set; } = "homelab-API";
    public string StatusTopic { get; set; } = "homelab/+/status";
}