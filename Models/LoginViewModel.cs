using System;
using System.ComponentModel.DataAnnotations;

namespace Hzg.Models;

/// <summary>
/// 登录模型
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// 用户名
    /// </summary>
    /// <value></value>
    [StringLength(32)]
    [Required(ErrorMessage = "请输入用户名！")]
    [Display(Name = "用户名")]
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <value></value>
    [Required(ErrorMessage = "请输入密码！")]
    [Display(Name = "密码")]
    public string Password { get; set; }

    /// <summary>
    /// 是否记住
    /// </summary>
    /// <value></value>
    [Display(Name = "是否记住")]
    public bool RememberMe { get; set; }

    /// <summary>
    /// 公司标识
    /// </summary>
    /// <value></value>
    public string CorpId { get; set; }
}