using System.Text;
using System.Security.Cryptography;

namespace Hzg.Tool;

/// <summary>
/// MQTT 工具类
/// </summary>
public static class MqttTool
{
    private static string accessSecret = "123";
    private static string accessKey = "123";
    /// <summary>
    /// 生成 ClientId
    /// </summary>
    /// <param name="corpId"></param>
    /// <param name="productKey"></param>
    /// <param name="deviceId"></param>
    /// <param name="accessKey"></param>
    /// <returns></returns>
    public static string GenerateClientId(string corpId, string productKey, string deviceId)
    {
        return corpId + "." + productKey + "." + deviceId + "|" + MqttTool.accessKey;
    }

    /// <summary>
    /// 生成 UserName
    /// </summary>
    /// <param name="corpId"></param>
    /// <param name="productKey"></param>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    public static string GenerateUsername(string corpId, string productKey, string deviceId)
    {
        return corpId + "&" + productKey + "&" + deviceId;
    }

    /// <summary>
    /// 生成 username
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="username"></param>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    public static string GeneratePassword(string clientId, string username)
    {
        var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(MqttTool.accessSecret));

        return Convert.ToBase64String(hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(username)));
    }

    /// <summary>
    /// 验证 MQTT 连接参数
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static bool VerifyMqtt(string clientId, string username, string password)
    {
        return GeneratePassword(clientId, username) == password;
    }
}