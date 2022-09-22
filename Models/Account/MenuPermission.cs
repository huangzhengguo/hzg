using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

/// <summary>
/// 导航菜单权限表
/// </summary>
public class MenuPermission : BaseEntity
{
    /// <summary>
    /// 根菜单标识
    /// </summary>
    /// <value></value>
    [Column("root_menu_id")]
    public Guid RootMenuId { get; set; }

    /// <summary>
    /// 末级菜单标识，也就是要设置权限的菜单，只有末级菜单能跳转
    /// </summary>
    /// <value></value>
    [Column("sub_menu_id")]
    public Guid SubMenuId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("user_name")]
    public string UserName { get; set; }

    /// <summary>
    /// 分组
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("user_group")]
    public string UserGroup { get; set; }
    
    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("user_role")]
    public string UserRole { get; set; }

    /// <summary>
    /// 是否可用
    /// </summary>
    /// <value></value>
    [Column("usable")]
    public bool? Usable { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    /// <value></value>
    [Column("visible")]
    public bool? Visible { get; set; }

    /// <summary>
    /// 对应菜单的 url
    /// </summary>
    /// <value></value>
    [Column("url")]
    public string Url { get; set; }
}