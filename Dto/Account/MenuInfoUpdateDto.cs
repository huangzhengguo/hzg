using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Hzg.Consts;

namespace Hzg.Dto;

/// <summary>
/// 过滤器表
/// </summary>
public class HzgMenuInfoUpdateDto
{
    /// <summary>
    /// 主键
    /// </summary>
    /// <value></value>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// 品牌
    /// </summary>
    /// <value></value>
    [Required]
    public string Title { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Name { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 上级菜单标识
    /// </summary>
    /// <value></value>
    public Guid? ParentMenuId { get; set; }

    /// <summary>
    /// 路由路径
    /// </summary>
    /// <value></value>
    [MaxLength(512)]
    public string Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    /// <value></value>
    [MaxLength(512)]
    public string ComponentPath { get; set; }

    /// <summary>
    /// 路由元数据
    /// </summary>
    /// <value></value>
    public string Meta { get; set; }
}