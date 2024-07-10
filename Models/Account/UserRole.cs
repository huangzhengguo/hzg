using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Models;

/// <summary>
/// 用户角色
/// </summary>
[Table("user_role")]
[Comment("用户角色关联表")]
public class HzgUserRole
{
    /// <summary>
    /// 用户标识
    /// </summary>
    /// <value></value>
    [Column("user_id")]
    [Comment("用户标识")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 角色标识
    /// </summary>
    /// <value></value>
    [Column("role_id")]
    [Comment("角色标识")]
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