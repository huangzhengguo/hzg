namespace Hzg.Const;

public static class ErrorCodeMessage
{
    public static string Message(ErrorCode errorCode)
    {
        switch (errorCode)
        {
            case ErrorCode.ErrorCode_Success:
                return "成功!";
            default:
                return "无";
        }
    }
}