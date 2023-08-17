using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace Hzg.Services;

/// <summary>
/// MQTT 服务
/// </summary>
public interface IMqttService
{
    /// <summary>
    /// 发布 MQTT 消息
    /// </summary>
    /// <param name="applicationMessage">MQTT 消息</param>
    /// <returns></returns>
    Task PublishAsync(MqttApplicationMessage applicationMessage);

    // Task SubscribeAsync(MqttClientSubscribeOptions options);
}