using System;
using System.ComponentModel.DataAnnotations;

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
    public string SortCode { get; set; }

    /// <summary>
    /// 路由名称
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    public string Name { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    public string Title { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    /// <value></value>
    public string Icon { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    /// <value></value>
    public string Description { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <value></value>
    public string Remark { get; set; }

    /// <summary>
    /// 上级菜单标识
    /// </summary>
    /// <value></value>
    public Guid? ParentMenuId { get; set; }

    /// <summary>
    /// 是否是第一级
    /// </summary>
    /// <value></value>
    public bool? IsRoot { get; set; }

    /// <summary>
    /// 是否是最后一级
    /// </summary>
    /// <value></value>
    public bool? IsFinal { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    public string Url { get; set; }

    /// <summary>
    /// 路由路径
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    public string Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    /// <value></value>
    [StringLength(512)]
    public string ComponentPath { get; set; }

    public string Meta { get; set; }
}