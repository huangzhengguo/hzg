namespace Hzg.Dto;

/// <summary>
/// 创建用户信息
/// </summary>
public class UserEditDto
{
    /// <summary>
    /// 标识
    /// </summary>
    /// <value></value>
    public string Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    /// <value></value>
    public string Name { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    /// <value></value>
    public string Gender { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    public string Email { get; set; }

    /// <summary>
    /// 分组标识
    /// </summary>
    /// <value></value>
    public string[] GroupIds { get; set; }

    /// <summary>
    /// 角色标识
    /// </summary>
    /// <value></value>
    public string[] RoleIds { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <value></value>
    public string Password { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    /// <value></value>
    public string Nickname { get; set; }
}