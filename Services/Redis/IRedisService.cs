namespace Hzg.Services;

/// <summary>
/// Redis 服务
/// </summary>
public interface IRedisService
{
    /// <summary>
    /// 设置字符串
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="seconds">有效期秒数</param>
    /// <returns></returns>
    bool SetStringValue(string key, string value, long seconds);

    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string GetStringValue(string key);

    /// <summary>
    /// 删除值
    /// </summary>
    /// <param name="key"></param>
    void DeleteStringValue(string key);

    /// <summary>
    /// 获取过期时间
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    DateTime? GetExpire(string key);

    /// <summary>
    /// 获取剩余过期剩余妙数
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    long GetRemainingSeconds(string key);
}