using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Hzg.Consts;

namespace Hzg.Vo;

/// <summary>
/// 分组信息
/// </summary>
public class HzgGroupInfoUpdateVo
{
    /// <summary>
    /// 标识
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父标识
    /// </summary>
    /// <value></value>
    public Guid? ParentId { get; set; }
}