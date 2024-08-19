using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hzg.Models;

/// <summary>
/// 用户状态
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// 离线
    /// </summary>
    Offline = 0,
    /// <summary>
    /// 在线
    /// </summary>
    Online = 1
}

/// <summary>
/// 用户类
/// </summary>
public class HzgUser : BaseAccount
{
    /// <summary>
    /// 用户名
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    [Column("name")]
    [Comment("用户名")]
    public string Name { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    [Column("nick_name")]
    [Comment("昵称")]
    public string Nickname { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <value></value>
    [StringLength(256)]
    [Column("role")]
    [Comment("角色")]
    public string Role { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    /// <value></value>
    [StringLength(16)]
    [Column("gender")]
    [Comment("性别")]
    public string Gender { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("password")]
    [Comment("密码")]
    public string Password { get; set; }

    /// <summary>
    /// 盐
    /// </summary>
    [Column("salt")]
    [Comment("盐")]
    public string Salt { get; set; }

    /// <summary>
    /// 支付密码
    /// </summary>
    [Column("pay_password")]
    [Comment("支付密码")]
    public string PayPassword { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    [StringLength(128)]
    [Column("email")]
    [Comment("邮箱")]
    public string Email { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [StringLength(32)]
    [Column("user_mobile")]
    [Comment("手机号码")]
    public string UserMobile { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    [Column("avatar")]
    [Comment("头像")]
    public string Avatar { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    /// <value></value>
    [Column("last_login_time")]
    [Comment("最后登录时间")]
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 谷歌FCM
    /// </summary>
    [Column("fcm_token")]
    [Comment("谷歌FCM")]
    public string FcmToken { get; set; }

    /// <summary>
    /// 苹果Token
    /// </summary>
    [Column("ios_token")]
    [Comment("苹果Token")]
    public string IosToken { get; set; }

    /// <summary>
    /// 在线状态(offline/online)
    /// </summary>
    [Column("在线状态")]
    [Comment("苹果Token")]
    public string OnlineState { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [StringLength(64)]
    [Column("real_name")]
    [Comment("真实姓名")]
    public string RealName { get; set; }
    
    /// <summary>
    /// 设置参数
    /// </summary>
    [Column("settings")]
    [Comment("设置参数")]
    public string Settings { get; set; }

    /// <summary>
    /// 注册时间
    /// </summary>
    /// <value></value>
    [Column("user_regtime")]
    [Comment("注册时间")]
    public DateTime? UserRegtime { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    /// <value></value>
    [Column("status")]
    [Comment("用户状态")]
    public UserStatus Status { get; set; }

    /// <summary>
    /// 用户角色
    /// </summary>
    /// <value></value>
    public virtual ICollection<HzgUserRole> UserRoles { get; set; }

    /// <summary>
    /// 用户分组
    /// </summary>
    /// <value></value>
    public virtual ICollection<HzgUserGroup> UserGroups { get; set; }
}