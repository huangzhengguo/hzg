using System.ComponentModel.DataAnnotations;

namespace Hzg.Models;

public enum UserStatus
{
    Online = 0,
    Offline = 1
}

/// <summary>
/// 用户类
/// </summary>
public class User : BaseAccount
{
    /// <summary>
    /// 用户名
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    public string Name { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    public string Nickname { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    /// <value></value>
    [StringLength(16)]
    public string Gender { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    public string Password { get; set; }

    /// <summary>
    /// 盐
    /// </summary>
    public string Salt { get; set; }

    /// <summary>
    /// 支付密码
    /// </summary>
    public string PayPassword { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    [StringLength(128)]
    public string Email { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string UserMobile { get; set; }

    /// <summary>
    /// 企业ID
    /// </summary>
    public string CorpId { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    /// <value></value>
    [StringLength(64)]
    public string Avatar { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    /// <value></value>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 谷歌-FCM
    /// </summary>
    public string SendFcmToken { get; set; }

    /// <summary>
    /// 苹果-IOS
    /// </summary>
    public string SendIosToken { get; set; }

    /// <summary>
    /// 在线状态(offline/online)
    /// </summary>
    public string Online { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; }
    
    /// <summary>
    /// 设置参数
    /// </summary>
    public string Settings { get; set; }

    public DateTime? UserRegtime { get; set; }
    public DateTime? ModifyTime { get; set; }

    public UserStatus status { get; set; }

    // 导航属性
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<UserGroup> UserGroups { get; set; }
}