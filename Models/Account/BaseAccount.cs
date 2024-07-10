using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
    [Comment("主键")]
    public KeyT Id { get; set; }

    /// <summary>
    /// 企业标识
    /// </summary>
    [Column("brand")]
    [MaxLength(32)]
    [Comment("企业标识")]
    public string Brand { get; set; }

    /// <summary>
    /// 组织标识
    /// </summary>
    [Column("organization")]
    [MaxLength(64)]
    [Comment("组织标识")]
    public string Organization { get; set; }

    /// <summary>
    /// 创建人标识
    /// </summary>
    /// <value></value>
    [Column("creator_id")]
    [Comment("创建人标识")]
    public KeyT CreatorId { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    /// <value></value>
    [StringLength(128)]
    [Column("create_user")]
    [Comment("创建人")]
    public string CreateUser { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("create_time")]
    [Comment("创建时间")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Column("update_time")]
    [Comment("修改时间")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    /// <value></value>
    [Column("des")]
    [Comment("描述")]
    public string Description { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <value></value>
    [Column("remark")]
    [Comment("备注")]
    public string Remark { get; set; }

    /// <summary>
    /// 排序编号
    /// </summary>
    /// <value></value>
    [Column("sort_num")]
    [Comment("排序编号")]
    public long SortNum { get; set; }

    /// <summary>
    /// 排序编码
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("sort_code")]
    [Comment("排序编码")]
    public string SortCode { get; set; }
}

/// <summary>
/// 账户基类
/// </summary>
public abstract class BaseAccount : BaseAccount<Guid>
{
    /// <summary>
    /// 最后修改人
    /// </summary>
    [Column("update_user")]
    public Guid? UpdateUser { get; set; }   
}