using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

/// <summary>
/// 职位
/// </summary>
public class HzgPosition : HzgBaseEntity
{
    /// <summary>
    /// 职位名称
    /// </summary>
    [StringLength(32)]
    [Column("name")]
    public string Name { get; set; }

    /// <summary>
    /// 职位编码
    /// </summary>
    [Column("code")]
    public string Code { get; set; }

    /// <summary>
    /// 顺序
    /// </summary>
    [Column("sort")]
    public int Sort { get; set; }

    /// <summary>
    /// 状态(字典)
    /// </summary>
    [Column("status")]
    public string Status { get; set; }
}