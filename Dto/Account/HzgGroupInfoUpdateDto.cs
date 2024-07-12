using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Hzg.Consts;

namespace Hzg.Dto;

/// <summary>
/// 分组更新
/// </summary>
public class HzgGroupInfoUpdateDto
{
    /// <summary>
    /// 标识
    /// </summary>
    /// <value></value>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Name { get; set; }

    /// <summary>
    /// 父标识
    /// </summary>
    /// <value></value>
    public Guid? ParentId { get; set; }
}