using System.ComponentModel.DataAnnotations;
using Hzg.Tool;
using Hzg.Consts;
using Hzg.Models;

namespace Hzg.Vo;

/// <summary>
/// 用户账号信息详情
/// </summary>
public class HzgUserVo
{
    /// <summary>
    /// 标识
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    /// <value></value>
    public string Name { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    public string Role { get; set; }

    /// <summary>
    /// 分组标识
    /// </summary>
    /// <value></value>
    public string GroupId { get; set; }

    /// <summary>
    /// 角色标识
    /// </summary>
    /// <value></value>
    public string RoleId { get; set; }

    /// <summary>
    /// 分组
    /// </summary>
    /// <value></value>
    public string Group { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    /// <value></value>
    public string Nickname { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    /// <value></value>
    public string Gender { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    public string Email { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    /// <value></value>
    public string Avatar { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    /// <value></value>
    public string UserMobile { get; set; }

    /// <summary>
    /// 上次登录时间
    /// </summary>
    /// <value></value>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 在线状态(offline/online)
    /// </summary>
    public string OnlineState { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    /// <value></value>
    public UserStatus Status { get; set; }

    /// <summary>
    /// 企业ID
    /// </summary>
    public String Brand { get; set; }
}