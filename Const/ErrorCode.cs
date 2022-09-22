using System;
using System.ComponentModel.DataAnnotations;

namespace Hzg.Const;

/// <summary>
/// 错误码常量：前三个数字错误分类，后三个数字标识具体错误
/// </summary>
public enum ErrorCode
{
    // 成功
    ErrorCode_Success = 20000,
    ErrorCode_Query_Success = 20001,
    ErrorCode_Create_Success = 20002,
    ErrorCode_Update_Success = 20003,
    ErrorCode_Confirm_Success = 20004,
    ErrorCode_Close_Success = 20005,

    // 失败
    ErrorCode_Failed = 20020,
    ErrorCode_Query_Failed = 20021,
    ErrorCode_Create_Failed = 20022,
    ErrorCode_Update_Failed = 20023,
    ErrorCode_Confirm_Failed = 20024,
    ErrorCode_Close_Failed = 20025,
    
    // 不存在
    ErrorCode_NotExist = 20040,

    #region 账号相关错误码
    [Display(Name = "登录出错")]
    ErrorCode_Login = 100000,
    [Display(Name = "用户不存在")]
    ErrorCode_UserNotExist = 100001,
    [Display(Name = "密码错误")]
    ErrorCode_Password = 100002,
    [Display(Name = "用户或密码错误")]
    ErrorCode_UserOrPassword = 100003,
    [Display(Name = "已存在")]
    ErrorCode_HasExisted = 100004,
    [Display(Name = "已引用")]
    ErrorCode_HasReferenced = 100005,
    ErrorCode_NoPermission = 100007,
    [Display(Name = "失败")]
    ErrorCode_HasClosed = 100008
    #endregion
}