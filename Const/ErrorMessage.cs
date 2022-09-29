namespace Hzg.Const;

/// <summary>
/// 错误信息
/// </summary>
public static class ErrorMessage
{
    /// <summary>
    /// 语言对照
    /// </summary>
    /// <value></value>
    public static Dictionary<string, string> Messages = new Dictionary<string, string>
    {
        { "createSuccess", "创建成功" }, // 创建成功
        { "getSuccess", "获取成功" }, // 获取成功
        { "thisUserAlreadyExists", "This user already exists!" }, // 用户已存在
        { "thisUserNotExists", "This User Not Exists!" },
        { "corpidDoesNotExist", "corpid does not exist!" }, // 公司 Id 不存在
        { "corpidCannotEmpty", "CorpId Can not Empty!" },   // 公司 Id 不能为空
        { "emailCannotEmpty", "Email Can not Empty!" }   // 邮箱不能为空
    };
}