using Hzg.Consts;

namespace Hzg.Services;

/// <summary>
/// 用户传输对象
/// </summary>
public class UserDto
{
    /// <summary>
    /// 用户id
    /// </summary>
    /// <value></value>
    public string Id { get; set; }

    /// <summary>
    /// 品牌
    /// </summary>
    public string Brand { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    /// <value></value>
    public string UserName { get; set; }

    /// <summary>
    /// 用户分组
    /// </summary>
    /// <value></value>
    public string Groups { get; set; }

    /// <summary>
    /// 用户角色
    /// </summary>
    /// <value></value>
    public string Roles { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    public string Email { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    /// <value></value>
    public string Phone { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    /// <value></value>
    public string Nickname { get; set; }
    
    /// <summary>
    /// 头像
    /// </summary>
    /// <value></value>
    public string Avatar { get; set; }

    /// <summary>
    /// 主题
    /// </summary>
    /// <value></value>
    public string Topic { get; set; }

    /// <summary>
    /// MQTT 客户端标识
    /// </summary>
    /// <value></value>
    public string MqttClientId { get; set; }

    /// <summary>
    /// MQTT 用户名
    /// </summary>
    /// <value></value>
    public string MqttUsername { get; set; }

    /// <summary>
    /// MQTT 密码
    /// </summary>
    /// <value></value>
    public string MqttPassword { get; set; }
}