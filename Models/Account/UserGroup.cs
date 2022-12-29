using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Models;

public class HzgUserGroup
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("group_id")]
    public Guid GroupId { get; set; }

    /// <summary>
    /// 是否是管理员
    /// </summary>
    /// <value></value>
    [Column("is_admin")]
    [Comment("是否是管理员")]
    public bool? IsAdmin { get; set; }

    public virtual HzgUser User { get; set; }
    public virtual HzgGroup Group { get; set; }
}