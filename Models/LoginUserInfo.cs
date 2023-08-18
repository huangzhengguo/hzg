using System;

namespace Hzg.Models;

/// <summary>
/// 登录用户信息
/// </summary>
/// <typeparam name="T"></typeparam>
public class LoginUserInfo<T>
{
    /// <summary>
    /// 用户id
    /// </summary>
    /// <value></value>
    public T UserId { get; set; }

    /// <summary>
    /// 品牌
    /// </summary>
    /// <value></value>
    public string Brand { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    /// <value></value>
    public string UserName { get; set; }

    /// <summary>
    /// 用户分组
    /// </summary>
    /// <value></value>
    public string Groups { get; set; }

    /// <summary>
    /// 用户角色
    /// </summary>
    /// <value></value>
    public string Roles { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <value></value>
    public string Email { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    /// <value></value>
    public string Phone { get; set; }
}

/// <summary>
/// 登录用户的信息
/// </summary>
public class LoginUserInfo: LoginUserInfo<Guid>
{
}