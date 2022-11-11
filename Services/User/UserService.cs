using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Hzg.Models;
using Hzg.Tool;
using Hzg.Consts;
using Hzg.Data;
using Hzg.Dto;

namespace Hzg.Services;

/// <summary>
/// 用户相关服务
/// </summary>
public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AccountDbContext _accountDbContext;
    private readonly ILocalizerService _localizerService;

    public UserService(IHttpContextAccessor httpContextAccessor,
                       AccountDbContext accountDbContext,
                       ILocalizerService localizerService)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountDbContext = accountDbContext;
        _localizerService = localizerService;
    }

    /// <summary>
    /// 获取当前登录用户信息
    /// </summary>
    /// <returns></returns>
    public async Task<LoginUserInfo> GetLoginUserInfo()
    {
        var user = _httpContextAccessor.HttpContext.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var loginUser = await _accountDbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id.ToString() == userId);
        if (loginUser != null)
        {
            var userInfo = new LoginUserInfo();

            userInfo.UserId = loginUser.Id;
            userInfo.Brand = loginUser.Brand;

            return userInfo;
        }

        return null;
    }

    /// <summary>
    /// 获取当前登录用户的名称
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetCurrentUserName()
    {
        var userInfo = await GetLoginUserInfo();
        
        return userInfo != null ? userInfo.UserName : null;
    }

    /// <summary>
    /// 获取登录用户的 Id
    /// </summary>
    /// <returns></returns>
    public async Task<Guid?> GetLoginUserId()
    {
        var userInfo = await GetLoginUserInfo();

        return userInfo != null ? userInfo.UserId : null;
    }

    /// <summary>
    /// 检测当前用户是否有效
    /// </summary>
    /// <returns></returns>
    public async Task<bool> CheckToken()
    {
        var currentUserId = await GetLoginUserId();
        if (currentUserId == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 获取登录用户所属公司
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetLoginUserCorpId()
    {
        var userInfo = await GetLoginUserInfo();

        return userInfo != null ? userInfo.Brand : null;
    }

    /// <summary>
    /// 获取登录用户的 Id
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetStringUserId()
    {
        var userInfo = await GetLoginUserInfo();
        if (userInfo != null)
        {
            return userInfo.UserId.ToString();
        }

        return null;
    }

    /// <summary>
    /// 用户是否存在
    /// </summary>
    /// <param name="brand">公司ID</param>
    /// <param name="email">邮箱</param>
    /// <returns></returns>
    public async Task<(bool, string)> IsUserExist(String brand, String email)
    {
        if (brand == null)
        {
            return (false, _localizerService.Localizer("corpidCannotEmpty"));
        }

        if (email == null)
        {
            return (false, _localizerService.Localizer("emailCannotEmpty"));
        }

        var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Brand == brand && u.Email == email);
        if (user != null)
        {
            return (true, _localizerService.Localizer("thisUserAlreadyExists"));
        }

        return (false, _localizerService.Localizer("thisUserNotExists"));
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="resetDto"></param>
    /// <returns></returns>
    public async Task<bool> ModifyPassword(ModifyDto modifyDto)
    {
        var userId = await GetLoginUserId();
        if (userId == null)
        {
            return false;
        }

        var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user.Password != MD5Tool.Encrypt(modifyDto.old_password, user.Salt))
        {
            return false;
        }

        user.Password = MD5Tool.Encrypt(modifyDto.new_password, user.Salt);

        _accountDbContext.Users.Update(user);

        if (await _accountDbContext.SaveChangesAsync() != 1)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="resetDto"></param>
    /// <returns></returns>
    public async Task<bool> ResetPassword(ResetDto resetDto)
    {
        var verifyCodeKey = CommonConstant.EMAIL_RESETPSW_CODE_KEY + resetDto.Email;
        var verifyCode = RedisTool.GetStringValue(verifyCodeKey);
        if (verifyCode != null && resetDto.VerifyCode == verifyCode)
        {
            // 删除 Redis 中的验证码
            RedisTool.DeleteStringValue(verifyCodeKey);

            var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Email == resetDto.Email && u.Brand == resetDto.Brand);
            
            user.Salt = RandomTool.GenerateDigitalAlphabetCode(6);
            user.Password = MD5Tool.Encrypt(resetDto.NewPassword, user.Salt);

            _accountDbContext.Users.Update(user);
            if (await _accountDbContext.SaveChangesAsync() != 1)
            {
                return false;
            }

            return true;
        }
        
        return false;
    }

    /// <summary>
    /// 修改用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="userEditDto"></param>
    /// <returns></returns>
    public async Task<bool> ModifyUser(Guid userId, UserEditDto userEditDto)
    {
        var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }

        CommonTool.CopyProperties(userEditDto, user);

        _accountDbContext.Users.Update(user);
        if (await _accountDbContext.SaveChangesAsync() != 1)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 用户注销账号
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<string> DeleteAccount(string userId)
    {
        var responseData = ResponseTool.FailedResponseData();
        var currentUserId = GetLoginUserId();
        if (currentUserId.ToString() != userId)
        {
            responseData.Code = ErrorCode.Illegal_Token;

            return JsonSerializerTool.SerializeDefault(responseData);
        }

        var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Id.ToString() == userId);
        if (user == null)
        {
            responseData.Code = ErrorCode.User_Not_Exist;

            return JsonSerializerTool.SerializeDefault(responseData); 
        }

        _accountDbContext.Users.Remove(user);
        if (await _accountDbContext.SaveChangesAsync() != 1)
        {
            return JsonSerializerTool.SerializeDefault(responseData); 
        }

        responseData.Code = ErrorCode.Success;

        return JsonSerializerTool.SerializeDefault(responseData); 
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseData<string>> Logout()
    {
        var responseData = ResponseTool.FailedResponseData<string>();
        var currentUserId = await GetLoginUserId();
        if (currentUserId == null) {
            responseData.Code = ErrorCode.Illegal_Token;

            return responseData;
        }

        responseData.Code = ErrorCode.Success;

        return responseData;
    }
}