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

    /// <summary>
    /// 检测妙数时间差是否超过指定天数
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    public static bool CheckSecondsRangeMoreThanDays(long startTime, long endTime, long days)
    {
        var timeSpan = new TimeSpan((endTime - startTime) * 10000000);
        if (timeSpan.Days > days)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 转换毫秒时间戳为日期
    /// </summary>
    /// <param name="timeStamp">毫秒时间戳</param>
    /// <returns></returns>
    public static DateTime FromLongTimeStamp(long timeStamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).DateTime;
    }
}