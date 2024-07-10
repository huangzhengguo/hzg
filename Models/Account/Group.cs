using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Models;

/// <summary>
/// 分组
/// </summary>
[Table("group")]
[Comment("分组")]
public class HzgGroup : BaseAccount
{
    /// <summary>
    /// 分组名称
    /// </summary>
    [Required]
    [StringLength(256)]
    [Column("name")]
    [Comment("分组名称")]
    public string Name { get; set; }

    /// <summary>
    /// 父分组标识
    /// </summary>
    /// <value></value>
    [Column("parent_id")]
    [Comment("父分组标识")]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 用户分组
    /// </summary>
    /// <value></value>
    public virtual ICollection<HzgUserGroup> UserGroups { get; set; }

    /// <summary>
    /// 分组角色
    /// </summary>
    /// <value></value>
    public virtual ICollection<HzgRoleGroup> GroupRoles { get; set; }
}