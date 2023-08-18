using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

/// <summary>
/// 用户角色
/// </summary>
public class HzgUserRole
{
    /// <summary>
    /// 用户标识
    /// </summary>
    /// <value></value>
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 角色标识
    /// </summary>
    /// <value></value>
    [Column("role_id")]
    public Guid RoleId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    /// <value></value>
    public virtual HzgUser User { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    public virtual HzgRole Role { get; set; }
}