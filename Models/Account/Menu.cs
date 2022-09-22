using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

/// <summary>
/// 导航菜单
/// </summary>
public class Menu : BaseEntity
{
    /// <summary>
    /// 排序编号
    /// </summary>
    /// <value></value>
    [StringLength(16)]
    [Column("sort_code")]
    public string SortCode { get; set; }

    /// <summary>
    /// 路由名称
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    [Column("name")]
    public string Name { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("title")]
    public string Title { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    /// <value></value>
    [Column("icon")]
    public string Icon { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    /// <value></value>
    [Column("des")]
    public string Description { get; set; }

    /// <summary>
    /// 上级菜单标识
    /// </summary>
    /// <value></value>
    [Column("parent_menu_id")]
    public Guid? ParentMenuId { get; set; }

    /// <summary>
    /// 是否是第一级
    /// </summary>
    /// <value></value>
    [Column("is_root")]
    public bool? IsRoot { get; set; }

    /// <summary>
    /// 是否是最后一级
    /// </summary>
    /// <value></value>
    [Column("is_final")]
    public bool? IsFinal { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    [Column("url")]
    public string Url { get; set; }

    /// <summary>
    /// 路由路径
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    [Column("path")]
    public string Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    [Column("component_path")]
    public string ComponentPath { get; set; }

    [Column("meta")]
    public string Meta { get; set; }
}