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
    /// UTC 当前时间毫秒数时间戳
    /// </summary>
    /// <returns></returns>
    public static long UtcNowUnixTimeMilliseconds()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }
}