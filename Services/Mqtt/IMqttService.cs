using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace Hzg.Services;

/// <summary>
/// MQTT 服务
/// </summary>
public interface IMqttService
{
    Task PublishAsync(MqttApplicationMessage applicationMessage);

    // Task SubscribeAsync(MqttClientSubscribeOptions options);
}