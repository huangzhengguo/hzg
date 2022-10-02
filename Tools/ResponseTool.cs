using Hzg.Const;

namespace Hzg.Tool;

/// <summary>
/// 返回数据工具
/// </summary>
public static class ResponseTool
{
    /// <summary>
    /// 获取成功返回数据
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResponseData SuccessResponse(ErrorCode errorCode = ErrorCode.Success, object data = null)
    {
        return new ResponseData()
        {
            Code = ErrorCode.Success
        };
    }

    /// <summary>
    /// 获取失败返回数据
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ResponseData FailedResponseData(ErrorCode errorCode = ErrorCode.Failed, object data = null)
    {
        return new ResponseData()
        {
            Code = errorCode
        };
    }
}

/// <summary>
/// 返回数据
/// </summary>
public class ResponseData
{
    /// <summary>
    /// 错误码
    /// </summary>
    /// <value></value>
    public ErrorCode Code { get; set; }

    /// <summary>
    /// 是否显示信息
    /// </summary>
    /// <value></value>
    public bool ShowMsg { get; set; } = false;

    /// <summary>
    /// 数据
    /// </summary>
    /// <value></value>
    public object Data { get; set; }

    /// <summary>
    /// 根据当前错误码获取错误信息
    /// </summary>
    /// <value></value>
    public string Message {
        get 
        {
            var msg = ErrorCodeTool.GetErrorMessage(Code);

            return msg;
        }
    }

    /// <summary>
    /// 数据数量
    /// </summary>
    /// <value></value>
    public int AllDataCount { get; set; }
}