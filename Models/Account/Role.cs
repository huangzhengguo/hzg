using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Models;

/// <summary>
/// 角色
/// </summary>
[Table("role")]
[Comment("角色")]
public class HzgRole : BaseAccount
{
    /// <summary>
    /// 名称
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("name")]
    [Comment("名称")]
    public string Name { get; set; }

    /// <summary>
    /// 用户角色
    /// </summary>
    /// <value></value>
    public virtual ICollection<HzgUserRole> UserRoles { get; set; }

    /// <summary>
    /// 角色分组
    /// </summary>
    /// <value></value>
    public virtual ICollection<HzgRoleGroup> RoleGroups { get; set; }
}