using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string Name { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    [Column("nick_name")]
    public string Nickname { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    /// <value></value>
    [StringLength(16)]
    [Column("gender")]
    public string Gender { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Column("password")]
    public string Password { get; set; }

    /// <summary>
    /// 盐
    /// </summary>
    [Column("salt")]
    public string Salt { get; set; }

    /// <summary>
    /// 支付密码
    /// </summary>
    [Column("pay_password")]
    public string PayPassword { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    [StringLength(128)]
    [Column("email")]
    public string Email { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [StringLength(32)]
    [Column("user_mobile")]
    public string UserMobile { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    [Column("avatar")]
    public string Avatar { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    /// <value></value>
    [Column("last_login_time")]
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 谷歌-FCM
    /// </summary>
    [Column("fcm_token")]
    public string FcmToken { get; set; }

    /// <summary>
    /// 苹果-IOS
    /// </summary>
    [Column("ios_token")]
    public string IosToken { get; set; }

    /// <summary>
    /// 在线状态(offline/online)
    /// </summary>
    [Column("online_state")]
    public string OnlineState { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [StringLength(64)]
    [Column("real_name")]
    public string RealName { get; set; }
    
    /// <summary>
    /// 设置参数
    /// </summary>
    [Column("settings")]
    public string Settings { get; set; }

    /// <summary>
    /// 注册时间
    /// </summary>
    /// <value></value>
    [Column("user_regtime")]
    public DateTime? UserRegtime { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    /// <value></value>
    [Column("status")]
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