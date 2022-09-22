using System.ComponentModel.DataAnnotations;

namespace Hzg.Dto;

/// <summary>
/// 
/// </summary>
public class ModifyDto
{

    public static long serialVersionUID = 1L;

    /**
     * 旧密码
     */
    [Required]
    public String old_password { get; set; }

    /**
     * 新密码
     */
    [Required]
    public String new_password { get; set; }
}
