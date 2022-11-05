using Hzg.Const;

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
    
    public string Avatar { get; set; }

    public string Topic { get; set; }
    public string MqttClientId { get; set; }
    public string MqttUsername { get; set; }
    public string MqttPassword { get; set; }
}