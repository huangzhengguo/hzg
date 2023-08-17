using Hzg.Consts;

namespace Hzg.Tool;

/// <summary>
/// 语言工具类
/// </summary>
public static class LocalizerTool
{
    /// <summary>
    /// 获取对应键值的语言文本
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="language">语言</param>
    /// <returns></returns>
    public static string Value(string key, string language = "en")
    {
        if (ErrorMessage.Messages.Keys.Contains(key) == false)
        {
            return key;
        }
        return ErrorMessage.Messages[key];
    }
}