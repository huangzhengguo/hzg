namespace Hzg.Tool;

/// <summary>
/// 时间工具类
/// </summary>
public class DateTimeTool
{
    /// <summary>
    /// UTC 当前时间秒数时间戳
    /// </summary>
    /// <returns></returns>
    public static long UtcNowUnixTimeSeconds()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 指定时间秒数时间戳
    /// </summary>
    /// <returns></returns>
    public static long UtcUnixTimeSeconds(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }

    /// <summary>
    /// UTC 当前时间毫秒数时间戳
    /// </summary>
    /// <returns></returns>
    public static long UtcNowUnixTimeMilliseconds()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 转换毫秒为 UTC 时间
    /// </summary>
    /// <param name="milliseconds">毫秒数</param>
    /// <param name="format">时间格式</param>
    /// <returns></returns>
    public static string MillisecondsToFormatDate(long milliseconds, string format = "yyyy-MM-dd HH:mm:ss")
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).UtcDateTime.ToString(format);
    }
}