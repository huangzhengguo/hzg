using System.Text;
using System.Security.Cryptography;


namespace Hzg.Tool;

/// <summary>
/// MD5 工具
/// </summary>
public static class MD5Tool
{
    /// <summary>
    /// 使用盐对字符串加密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public static string Encrypt(string str, string salt)
    {
        var bytes = Encoding.UTF8.GetBytes(str + salt);

        var md5Bytes = MD5.HashData(bytes);
        string result = ""; // 32 字符，16 进制格式
        foreach (var m in md5Bytes)
        {
            result += string.Format("{0:x2}", m);
        }

        return result;
    }
}