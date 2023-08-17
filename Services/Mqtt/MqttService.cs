using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using MQTTnet;
using MQTTnet.Client;
using Hzg.Tool;

namespace Hzg.Services;

/// <summary>
/// MQTT 服务
/// </summary>
public class MqttService : IMqttService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="configuration">配置</param>
    public MqttService(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    private IMqttClient emqxMqttClient = null;

    /// <summary>
    /// 发布主题消息
    /// </summary>
    /// <param name="applicationMessage"></param>
    /// <returns></returns>
    public async Task PublishAsync(MqttApplicationMessage applicationMessage)
    {
        if (emqxMqttClient == null)
        {
            Debug.WriteLine("为空 MQTT 创建连接!");
            emqxMqttClient = await ConnectEmqxServer();
        }

        if (emqxMqttClient.IsConnected == false)
        {
            Debug.WriteLine("未连接 MQTT 重连!");
            await MqttTool.Reconnect(emqxMqttClient, emqxMqttClient.Options);
        }

        var result = await emqxMqttClient.PublishAsync(applicationMessage);
        if (result.IsSuccess == true)
        {
            
        }
        else
        {

        }
    }

    /// <summary>
    /// 初始化 MQTT 客户端
    /// </summary>
    /// <returns></returns>
    private async Task InitEmqxMqttClient()
    {
        emqxMqttClient = await ConnectEmqxServer();
    }

    /// <summary>
    /// 连接到 EMQX
    /// </summary>
    /// <returns></returns>
    private async Task<IMqttClient> ConnectEmqxServer()
    {
        Debug.WriteLine("初始化 MQTT 客户端!");
        var clientId = "iot-server-" + Guid.NewGuid().ToString();
        var username = "iot-server-" + Guid.NewGuid().ToString();
        var mqttClient = await MqttTool.ConnectMqttServer(_configuration["mqtt:host"], Convert.ToInt32(_configuration["mqtt:port"]), clientId, username, MqttTool.GeneratePassword(clientId, username));

        mqttClient.DisconnectedAsync += async (e) => {
            Debug.WriteLine("MQTT 断开重连!");
            // 重新连接 MQTT 服务器
            await MqttTool.Reconnect(mqttClient, mqttClient.Options);
        };

        return mqttClient;
    }

}