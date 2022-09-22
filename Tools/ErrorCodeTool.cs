using System;
using Hzg.Const;
using Hzg.Data;

namespace Hzg.Tool;

public static class ErrorCodeTool
{
    /// <summary>
    /// 根据错误码获取错误信息
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    public static string GetErrorMessage(ErrorCode errorCode)
    {
        string message = null;

        switch (errorCode)
        {
            case ErrorCode.ErrorCode_Success:
                message ="成功";
                break;
            case ErrorCode.ErrorCode_Create_Success:
                message = "创建成功";
                break;
            case ErrorCode.ErrorCode_Update_Success:
                message = "修改成功";
                break;
            case ErrorCode.ErrorCode_Confirm_Success:
                message = "确认成功";
                break;
            case ErrorCode.ErrorCode_Close_Success:
                message = "关闭成功";
                break;
            case ErrorCode.ErrorCode_Create_Failed:
                message = "创建失败";
                break;
            case ErrorCode.ErrorCode_Update_Failed:
                message = "修改失败";
                break;
            case ErrorCode.ErrorCode_Confirm_Failed:
                message = "确认失败";
                break;
            case ErrorCode.ErrorCode_Close_Failed:
                message = "关闭失败";
                break;
            case ErrorCode.ErrorCode_HasClosed:
                message = "已关闭";
                break;
            case ErrorCode.ErrorCode_NotExist:
                message = "不存在";
                break;
            default:
                message = null;
                break;
        }

        return message;
    }
}