using System.ComponentModel.DataAnnotations;

namespace Hzg.Dto;

/// <summary>
/// 
/// </summary>
public class ModifyDto
{
    /// <summary>
    /// 旧密码
    /// </summary>
    /// <value></value>
    [Required]
    public String OldPassword { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    /// <value></value>
    [Required]
    public String NewPassword { get; set; }
}
