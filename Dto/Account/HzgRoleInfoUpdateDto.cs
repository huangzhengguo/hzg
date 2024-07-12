using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Hzg.Consts;

namespace Hzg.Dto;

/// <summary>
/// 角色更新
/// </summary>
public class HzgRoleInfoUpdateDto
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
}