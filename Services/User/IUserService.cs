using System;
using System.Threading.Tasks;
using Hzg.Models;
using Hzg.Dto;

namespace Hzg.Services;

/// <summary>
/// 用户功能
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 获取当前登录用户的信息
    /// </summary>
    /// <returns></returns>
    Task<LoginUserInfo> GetLoginUserInfo();
    
    /// <summary>
    /// 获取当前用户名
    /// </summary>
    /// <returns></returns>
    Task<string> GetCurrentUserName();

    /// <summary>
    /// 获取当前用户 Id
    /// </summary>
    /// <returns></returns>
    Task<Guid> GetCurrentUserId();

    /// <summary>
    /// 用户是否存在
    /// </summary>
    /// <param name="corpid">公司ID</param>
    /// <param name="email">邮箱</param>
    /// <returns></returns>
    Task<(bool isExist, string errorMessage)> IsUserExist(String corpid, String email);

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="resetDto"></param>
    /// <returns></returns>
    Task<bool> ModifyPassword(ModifyDto modifyDto);

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="resetDto"></param>
    /// <returns></returns>
    Task<bool> ResetPassword(ResetDto resetDto);

    /// <summary>
    /// 修改用户信息
    /// </summary>
    /// <param name="userid"></param>
    /// <param name="userEditDto"></param>
    /// <returns></returns>
    Task<bool> ModifyUser(Guid userid, UserEditDto userEditDto);
}