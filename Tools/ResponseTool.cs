using Hzg.Consts;

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
    /// 获取成功返回数据
    /// </summary>
    /// <param name="errorCode">错误码</param>
    /// <returns></returns>
    public static ResponseData<T> SuccessResponse<T>(ErrorCode errorCode = ErrorCode.Success, bool showMsg = false)
    {
        return new ResponseData<T>()
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
    public static ResponseData FailedResponseData(ErrorCode errorCode = ErrorCode.Failed, object data = null, bool showMsg = false)
    {
        return new ResponseData()
        {
            Code = errorCode,
            ShowMsg = showMsg
        };
    }

    /// <summary>
    /// 获取失败返回数据
    /// </summary>
    /// <param name="errorCode">错误码</param>
    /// <param name="showMsg">是否显示错误</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResponseData<T> FailedResponseData<T>(ErrorCode errorCode = ErrorCode.Failed, bool showMsg = false)
    {
        return new ResponseData<T>()
        {
            Code = errorCode,
            ShowMsg = showMsg
        };
    }
}

/// <summary>
/// 返回数据
/// </summary>
public class ResponseData<T>
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
    public T Data { get; set; }

    private string _messsage { get; set; }

    /// <summary>
    /// 根据当前错误码获取错误信息
    /// </summary>
    /// <value></value>
    public string Message {
        get 
        {
            if (_messsage == null)
            {
                return ErrorCodeTool.GetErrorMessage(Code);
            }

            return _messsage;
        }

        set
        {
            _messsage = value;
        }
    }

    /// <summary>
    /// 总数量
    /// </summary>
    /// <value></value>
    public int AllDataCount { get; set; }
}

/// <summary>
/// 返回数据，默认 objcet
/// </summary>
public class ResponseData : ResponseData<object>
{
}