using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Hzg.Models;
using Hzg.Tool;
using Hzg.Const;
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

        var currentUser = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Id.ToString() == userId);
        var userInfo = new LoginUserInfo();
        if (currentUser != null)
        {
            // userInfo.UserName = currentUser.Name;
            userInfo.UserId = currentUser.Id;
            // userInfo.Groups = currentUser.Group;
            // userInfo.Roles = currentUser.Role;
        }
        else
        {
            userInfo.UserName = "用户";
            userInfo.UserId = null;
        }

        return userInfo;
    }

    /// <summary>
    /// 获取当前登录用户的名称
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetCurrentUserName()
    {
        var userInfo = await GetLoginUserInfo();

        return userInfo.UserName;
    }

    /// <summary>
    /// 获取登录用户的 Id
    /// </summary>
    /// <returns></returns>
    public async Task<Guid?> GetCurrentUserId()
    {
        var userInfo = await GetLoginUserInfo();

        return userInfo.UserId;
    }

    /// <summary>
    /// 获取登录用户的 Id
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetStringUserId()
    {
        var userInfo = await GetLoginUserInfo();

        return userInfo.UserId?.ToString();
    }

    /// <summary>
    /// 用户是否存在
    /// </summary>
    /// <param name="corpid">公司ID</param>
    /// <param name="email">邮箱</param>
    /// <returns></returns>
    public async Task<(bool, string)> IsUserExist(String corpid, String email)
    {
        if (corpid == null)
        {
            return (false, _localizerService.Localizer("corpidCannotEmpty"));
        }

        if (email == null)
        {
            return (false, _localizerService.Localizer("emailCannotEmpty"));
        }

        var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.CorpId == corpid && u.Email == email);
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
        var loginUserInfo = await GetLoginUserInfo();
        if (loginUserInfo == null)
        {
            return false;
        }

        var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Id == loginUserInfo.UserId);
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

            var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Email == resetDto.Email && u.CorpId == resetDto.CorpId);
            
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
        var currentUserId = GetCurrentUserId();
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
}