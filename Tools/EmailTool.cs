using System.Text.RegularExpressions;

namespace Hzg.Tool;

/// <summary>
/// 邮箱工具类
/// </summary>
public static class EmailTool
{
    /// <summary>
    /// 邮箱正则表达式
    /// </summary>
    private static string emailPattern = "^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";

    /// <summary>
    /// 验证邮箱格式
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool ValidateEmail(string email)
    {
        Regex regex = new Regex(emailPattern);

        return regex.Match(email).Success;
    }
}