using System;
using System.ComponentModel.DataAnnotations;

namespace Hzg.Models;

/// <summary>
/// 登录模型
/// </summary>
public class LoginViewModel
{
    ///
    /// 用户名
    ///
    [StringLength(32)]
    [Required(ErrorMessage = "请输入用户名！")]
    [Display(Name = "用户名")]
    public string UserName { get; set; }

    ///
    /// 密码
    ///
    [Required(ErrorMessage = "请输入密码！")]
    [Display(Name = "密码")]
    public string Password { get; set; }

    ///
    /// 是否记住
    ///
    [Display(Name = "是否记住")]
    public bool RememberMe { get; set; }
}