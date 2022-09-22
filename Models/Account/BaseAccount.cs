using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

/// <summary>
/// 账号基类
/// </summary>
public abstract class BaseAccount<KeyT>
{
    /// <summary>
    /// 主键
    /// </summary>
    /// <value></value>
    [Column("id")]
    public KeyT Id { get; set; }

    /// <summary>
    /// 创建人 ID
    /// </summary>
    /// <value></value>
    [Column("creator_id")]
    public KeyT CreatorId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    [Column("create_user")]
    public string CreateUser { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("create_time")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Column("update_time")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <value></value>
    [Column("remark")]
    public string Remark { get; set; }
}

public abstract class BaseAccount : BaseAccount<Guid>
{
    /// <summary>
    /// 最后修改人
    /// </summary>
    [Column("update_user")]
    public Guid? UpdateUser { get; set; }   
}