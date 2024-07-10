using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Models;

/// <summary>
/// 用户分组关联
/// </summary>
[Table("user_group")]
[Comment("用户分组关联表")]
public class HzgUserGroup
{
    /// <summary>
    /// 用户标识
    /// </summary>
    /// <value></value>
    [Column("user_id")]
    [Comment("用户标识")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 分组标识
    /// </summary>
    /// <value></value>
    [Column("group_id")]
    [Comment("分组标识")]
    public Guid GroupId { get; set; }

    /// <summary>
    /// 是否是管理员
    /// </summary>
    /// <value></value>
    [Column("is_admin")]
    [Comment("是否是管理员")]
    public bool? IsAdmin { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    /// <value></value>
    public virtual HzgUser User { get; set; }

    /// <summary>
    /// 分组
    /// </summary>
    /// <value></value>
    public virtual HzgGroup Group { get; set; }
}