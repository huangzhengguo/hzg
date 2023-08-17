using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace Hzg.Tool;

/// <summary>
/// MQTT 工具类
/// </summary>
public static class MqttTool
{
    private static string accessSecret = "123456789";
    private static string accessKey = "987654321";
    
    /// <summary>
    /// 生成 ClientId
    /// </summary>
    /// <param name="brand">品牌</param>
    /// <param name="productKey">产品密钥</param>
    /// <param name="deviceId">设备标识</param>
    /// <returns></returns>
    public static string GenerateClientId(String brand, string productKey, string deviceId)
    {
        return brand + "." + productKey + "." + deviceId + "|" + MqttTool.accessKey;
    }

    /// <summary>
    /// 生成 UserName
    /// </summary>
    /// <param name="brand"></param>
    /// <param name="productKey"></param>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    public static string GenerateUsername(String brand, string productKey, string deviceId)
    {
        return brand + "&" + productKey + "&" + deviceId;
    }

    /// <summary>
    /// 生成 username
    /// </summary>
    /// <param name="clientId">客户端标识</param>
    /// <param name="username">用户名</param>
    /// <returns></returns>
    public static string GeneratePassword(string clientId, string username)
    {
        var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(MqttTool.accessSecret));

        return Convert.ToBase64String(hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(username)));
    }

    /// <summary>
    /// 生成设备的 MQTT 参数
    /// </summary>
    /// <param name="brand">品牌</param>
    /// <param name="productKey">产品密钥</param>
    /// <param name="deviceId">设备标识</param>
    /// <returns></returns>
    public static (string clientid, string username, string password) GenerateMqttParam(String brand, string productKey, string deviceId)
    {
        var clientid = MqttTool.GenerateClientId(brand, productKey, deviceId);
        var username = MqttTool.GenerateUsername(brand, productKey, deviceId);
        var password = MqttTool.GeneratePassword(clientid, username);

        return (clientid, username, password);
    }

    /// <summary>
    /// 验证 MQTT 连接参数
    /// </summary>
    /// <param name="clientId">客户端标识</param>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static bool VerifyMqtt(string clientId, string username, string password)
    {
        return GeneratePassword(clientId, username) == password;
    }

    /// <summary>
    /// 创建 MQTT 客户端并连接服务器
    /// </summary>
    /// <param name="server">MQTT 服务器</param>
    /// <param name="port">MQTT 服务器端口</param>
    /// <param name="clientId">客户端标识</param>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static async Task<IMqttClient> ConnectMqttServer(string server, int? port, string clientId, string username, string password)
    {
        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(server, port).Build();

        mqttClientOptions.ProtocolVersion = MqttProtocolVersion.V311;
        mqttClientOptions.ClientId = clientId;
        mqttClientOptions.Credentials = new MqttClientCredentials(username, Encoding.UTF8.GetBytes(password));
        mqttClientOptions.KeepAlivePeriod = TimeSpan.FromSeconds(1800);

        await Reconnect(mqttClient, mqttClientOptions);

        return mqttClient;
    }

    /// <summary>
    /// 重连服务器
    /// </summary>
    /// <param name="mqttClient">MQTT 客户端</param>
    /// <param name="mqttClientOptions">MQTT 客户端配置</param>
    /// <returns></returns>
    public static async Task Reconnect(IMqttClient mqttClient, MqttClientOptions mqttClientOptions)
    {
        await Task.Run(
            async () =>
            {
                while(mqttClient.IsConnected == false)
                {
                    try
                    {
                        if (!await mqttClient.TryPingAsync())
                        {
                            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                            Debug.WriteLine("连接成功!");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    finally
                    {
                        if (mqttClient.IsConnected == false)
                        {
                            Debug.WriteLine("重新连接!");
                            await Task.Delay(TimeSpan.FromSeconds(5));
                        }
                    }
                }
            }
        );
    }
}