using System.ComponentModel.DataAnnotations;

namespace Hzg.Dto;

/// <summary>
/// 重置密码信息
/// </summary>
public class ResetUserPasswordDto
{
    /// <summary>
    /// 企业ID
    /// </summary>
    /// <value></value>
    [Required]
    public string Brand { get; set; }

    /// <summary>
    /// 用户 ID
    /// </summary>
    /// <value></value>
    [Required]
    public string Id { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    /// <value></value>
    [Required]
    public string Password { get; set; }
}
