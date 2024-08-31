using System.ComponentModel.DataAnnotations;

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
    /// 企业标识
    /// </summary>
    [MaxLength(32)]
    public string Brand { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    /// <value></value>
    public string Name { get; set; }

    /// <summary>
    /// 分组标识
    /// </summary>
    /// <value></value>
    public string GroupId { get; set; }

    /// <summary>
    /// 角色标识
    /// </summary>
    /// <value></value>
    public string RoleId { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <value></value>
    public string Password { get; set; }
}