using System.ComponentModel.DataAnnotations;

namespace Hzg.Dto;

/// <summary>
/// 
/// </summary>
public class ResetDto
{
    /// <summary>
    /// 企业ID
    /// </summary>
    /// <value></value>
    [Required]
    public String CorpId { get; set; }

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
