using System.ComponentModel.DataAnnotations;

namespace Hzg.Dto;

/// <summary>
/// 重置密码信息
/// </summary>
public class ResetDto
{
    /// <summary>
    /// 企业ID
    /// </summary>
    /// <value></value>
    [Required]
    public String Brand { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    [Required]
    public String Email { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    /// <value></value>
    [Required]
    public String VerifyCode { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    /// <value></value>
    [Required]
    public String NewPassword { get; set; }
}
