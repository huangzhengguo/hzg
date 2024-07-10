using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Models;

/// <summary>
/// 角色分组
/// </summary>
[Table("role_group")]
[Comment("角色分组关联表")]
public class HzgRoleGroup
{
    /// <summary>
    /// 角色标识
    /// </summary>
    /// <value></value>
    [Column("role_id")]
    public Guid RoleId { get; set; }

    /// <summary>
    /// 分组标识
    /// </summary>
    /// <value></value>
    [Column("group_id")]
    public Guid GroupId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    public virtual HzgRole Role { get; set; }

    /// <summary>
    /// 分组
    /// </summary>
    /// <value></value>
    public virtual HzgGroup Group { get; set; }
}