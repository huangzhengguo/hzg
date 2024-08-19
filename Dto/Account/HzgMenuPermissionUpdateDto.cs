using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Dto;

/// <summary>
/// 菜单权限更新信息
/// </summary>
public class HzgMenuPermissionUpdateDto
{
    /// <summary>
    /// 分组标识
    /// </summary>
    /// <value></value>
    public string GroupId { get; set; }

    /// <summary>
    /// 分组中的角色标识
    /// </summary>
    /// <value></value>
    public Guid RoleId { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    /// <value></value>
    [Required]
    [StringLength(64)]
    public string Role { get; set; }

    /// <summary>
    /// 菜单标识列表
    /// </summary>
    /// <value></value>
    public string[] MenuIds { get; set; }
}