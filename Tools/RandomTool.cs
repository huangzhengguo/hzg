namespace Hzg.Tool;

/// <summary>
/// 随机字符串生成工具
/// </summary>
public static class RandomTool
{
    /// <summary>
    /// 生成指定长度的数字字符串
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateDigitalCode(int length = 4)
    {
        if (length < 1)
        {
            throw new ArgumentOutOfRangeException(string.Format("length {0} 不能小于 1", length));
        }

        var minValue = Math.Pow(10, length - 1);
        if (minValue > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(string.Format("minValue 超出范围 {0}", int.MaxValue));
        }

        var maxValue = Math.Pow(10, length) - 1;
        if (maxValue > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(string.Format("maxValue 超出范围 {0}", int.MaxValue));
        }

        return new Random().Next(Convert.ToInt32(minValue), Convert.ToInt32(maxValue)).ToString();
    }

    /// <summary>
    /// 生成指定长度的数字字母字符串
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateDigitalAlphabetCode(int length = 6)
    {
        if (length < 1)
        {
            throw new ArgumentOutOfRangeException(string.Format("length {0} 不能小于 1", length));
        }

        var minValue = Math.Pow(10, length - 1);
        if (minValue > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(string.Format("minValue 超出范围 {0}", int.MaxValue));
        }

        var maxValue = Math.Pow(10, length) - 1;
        if (maxValue > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(string.Format("maxValue 超出范围 {0}", int.MaxValue));
        }

        string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string code = "";
        for(int i=0;i<length;i++)
        {
            code = code + str[new Random().Next(0, str.Length)];
        }

        return code;
    }
}