using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Models;

/// <summary>
/// 菜单
/// </summary>
[Comment("菜单")]
[Table("menu")]
public class HzgMenu : BaseAccount
{
    /// <summary>
    /// 路由名称
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    [Column("name")]
    [Comment("路由名称")]
    public string Name { get; set; }

    /// <summary>
    /// 菜单标题
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("title")]
    [Comment("菜单标题")]
    public string Title { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    /// <value></value>
    [Column("icon")]
    [Comment("菜单图标")]
    public string Icon { get; set; }

    /// <summary>
    /// 上级菜单标识
    /// </summary>
    /// <value></value>
    [Column("parent_menu_id")]
    [Comment("上级菜单标识")]
    public Guid? ParentMenuId { get; set; }

    /// <summary>
    /// 是否是第一级
    /// </summary>
    /// <value></value>
    [Column("is_root")]
    [Comment("是否是第一级")]
    public bool? IsRoot { get; set; }

    /// <summary>
    /// 是否是最后一级
    /// </summary>
    /// <value></value>
    [Column("is_final")]
    [Comment("是否是最后一级")]
    public bool? IsFinal { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    [Column("url")]
    [Comment("链接")]
    public string Url { get; set; }

    /// <summary>
    /// 路由路径
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    [Column("path")]
    [Comment("路由路径")]
    public string Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    [Column("component_path")]
    [Comment("组件路径")]
    public string ComponentPath { get; set; }

    /// <summary>
    /// 路由元数据
    /// </summary>
    /// <value></value>
    [Column("meta")]
    [Comment("路由元数据")]
    public string Meta { get; set; }
}