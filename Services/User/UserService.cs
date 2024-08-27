using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Hzg.Models;
using Hzg.Tool;
using Hzg.Consts;
using Hzg.Data;
using Hzg.Dto;
using Hzg.Vo;

namespace Hzg.Services;

/// <summary>
/// 用户相关服务
/// </summary>
public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AccountDbContext _accountDbContext;
    private readonly ILocalizerService _localizerService;
    private readonly IRedisService _redisService;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="accountDbContext"></param>
    /// <param name="localizerService"></param>
    /// <param name="redisService"></param>
    public UserService(IHttpContextAccessor httpContextAccessor,
                       AccountDbContext accountDbContext,
                       ILocalizerService localizerService,
                       IRedisService redisService)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountDbContext = accountDbContext;
        _localizerService = localizerService;
        _redisService = redisService;
    }

    /// <summary>
    /// 获取当前登录用户所属品牌
    /// </summary>
    /// <returns></returns>
    public string GetCurrentUserBrand()
    {
        var user = _httpContextAccessor.HttpContext.User;
        return user.FindFirst("brand")?.Value;
    }

    /// <summary>
    /// 获取当前登录用户信息
    /// </summary>
    /// <returns></returns>
    public async Task<LoginUserInfo> GetLoginUserInfo()
    {
        var user = _httpContextAccessor.HttpContext.User;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var brand = user.FindFirst("brand")?.Value;

        // var loginUser = await _accountDbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id.ToString() == userId);
        // if (loginUser != null)
        // {
        //     var userInfo = new LoginUserInfo();

        //     userInfo.UserId = loginUser.Id;
        //     userInfo.Brand = loginUser.Brand;

        //     return userInfo;
        // }

        var userInfo = new LoginUserInfo();

        userInfo.UserId = new Guid(userId);
        userInfo.Brand = brand;

        return userInfo;
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
    /// <param name="modifyDto">修改信息</param>
    /// <returns></returns>
    public async Task<bool> ModifyPassword(ModifyDto modifyDto)
    {
        var userId = await GetLoginUserId();
        if (userId == null)
        {
            return false;
        }

        var user = await _accountDbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user.Password != MD5Tool.Encrypt(modifyDto.OldPassword, user.Salt))
        {
            return false;
        }

        user.Password = MD5Tool.Encrypt(modifyDto.NewPassword, user.Salt);

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
        var verifyCode = _redisService.GetStringValue(verifyCodeKey);
        if (verifyCode != null && resetDto.VerifyCode == verifyCode)
        {
            // 删除 Redis 中的验证码
            _redisService.DeleteStringValue(verifyCodeKey);

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

        HZG_CommonTool.CopyProperties(userEditDto, user);

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

    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="brand">品牌</param>
    /// <param name="page">当前页</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="keywords">查询关键词</param>
    /// <returns></returns>
    public async Task<ResponseData<IEnumerable<HzgUser>>> List(string brand, int page, int pageSize, string keywords)
    {
        var responseData = ResponseTool.FailedResponseData<IEnumerable<HzgUser>>();
        var data = await PagedList<HzgUser>.ListAsync(_accountDbContext.Users.AsNoTracking().Where(u => u.Brand == brand), page, pageSize);

        responseData.Code = ErrorCode.Success;
        responseData.Data = data;

        return responseData;
    }
}