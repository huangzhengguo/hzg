using Hzg.Const;

namespace Hzg.Tool;

/// <summary>
/// 返回固定格式
/// </summary>
public class ResponseData
{
    private string message = null;

    public ErrorCode Code { get; set; }
    public object Data { get; set; }
    public string Message {
        get 
        {
            if (this.message != null)
            {
                return this.message;
            }

            var msg = ErrorCodeTool.GetErrorMessage(Code);

            return msg;
        }

        set
        {
            this.message = value;
        }
    }
    public int AllDataCount { get; set; }
}