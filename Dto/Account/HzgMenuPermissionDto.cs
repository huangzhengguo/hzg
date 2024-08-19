using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Dto;

/// <summary>
/// 菜单权限信息
/// </summary>
public class HzgMenuPermissionDto
{
    /// <summary>
    /// 菜单标识
    /// </summary>
    /// <value></value>
    public Guid MenuId { get; set; }

    /// <summary>
    /// 根菜单标识
    /// </summary>
    /// <value></value>
    public Guid RootMenuId { get; set; }

    /// <summary>
    /// 末级菜单标识，也就是要设置权限的菜单，只有末级菜单能跳转
    /// </summary>
    /// <value></value>
    public Guid SubMenuId { get; set; }
    
    /// <summary>
    /// 分组
    /// </summary>
    /// <value></value>
    public Guid? GroupId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    public Guid? RoleId { get; set; }

    /// <summary>
    /// 是否可用
    /// </summary>
    /// <value></value>
    public bool? Usable { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    /// <value></value>
    public bool? Visible { get; set; }

    /// <summary>
    /// 对应菜单的 url
    /// </summary>
    /// <value></value>
    public string Url { get; set; }
}