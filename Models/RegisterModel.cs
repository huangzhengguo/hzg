using System.ComponentModel.DataAnnotations;

namespace Hzg.Models;

///
/// 登录模型
/// 
public class RegisterModel
{
    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <value></value>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    /// <value></value>
    [Required]
    public string VerifyCode { get; set; }
}