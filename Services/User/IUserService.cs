using Hzg.Models;
using Hzg.Dto;
using Hzg.Tool;

namespace Hzg.Services;

/// <summary>
/// 用户功能
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 获取当前登录用户所属品牌
    /// </summary>
    /// <returns></returns>
    string GetCurrentUserBrand();

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
    Task<Guid?> GetLoginUserId();

    /// <summary>
    /// 检测当前用户是否有效
    /// </summary>
    /// <returns></returns>
    Task<bool> CheckToken();

    /// <summary>
    /// 获取登录用户所属公司
    /// </summary>
    /// <returns></returns>
    Task<string> GetLoginUserCorpId();

    /// <summary>
    /// 获取当前用户 Id
    /// </summary>
    /// <returns></returns>
    Task<string> GetStringUserId();

    /// <summary>
    /// 用户是否存在
    /// </summary>
    /// <param name="brand">公司ID</param>
    /// <param name="email">邮箱</param>
    /// <returns></returns>
    Task<(bool isExist, string errorMessage)> IsUserExist(String brand, String email);

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="modifyDto">重置密码信息</param>
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

    /// <summary>
    /// 用户注销账号
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<string> DeleteAccount(string userId);

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <returns></returns>
    Task<ResponseData<string>> Logout();
}