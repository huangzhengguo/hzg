using Hzg.Const;

namespace Hzg.Dto;

/// <summary>
/// 创建用户信息
/// </summary>
public class UserInfoDto
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    /// <value></value>
    public Guid? Id { get; set; }

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
    /// 分组信息
    /// </summary>
    /// <value></value>
    public Guid[] GroupIds { get; set; }

    /// <summary>
    /// 角色信息
    /// </summary>
    /// <value></value>
    public Guid[] RoleIds { get; set; }

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
    /// 头像路径
    /// </summary>
    /// <value></value>
    public string AvatarPath { get; set; }
}