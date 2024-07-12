using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Hzg.Consts;

namespace Hzg.Dto;

/// <summary>
/// 菜单
/// </summary>
public class HzgMenuInfoDto
{
    /// <summary>
    /// 标题
    /// </summary>
    /// <value></value>
    [Required]
    [MaxLength(32)]
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