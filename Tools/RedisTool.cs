using StackExchange.Redis;
using Hzg.Consts;

namespace Hzg.Tool;

public static class RedisTool
{
    private static ConnectionMultiplexer _redis;
    public static ConnectionMultiplexer Redis
    {
        get {
            if (_redis == null)
            {
                var options = new ConfigurationOptions
                {
                    EndPoints = { CommonConstant.RedisServer }
                };

                options.Password = "Cyd@2020";

                _redis = ConnectionMultiplexer.Connect(options);
            }

            return _redis;
        }
    }

    /// <summary>
    /// 设置字符串
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool SetStringValue(string key, string value, long seconds)
    {
        ConnectionMultiplexer redis = RedisTool.Redis;
        IDatabase db = redis.GetDatabase();

        var result = db.StringSet(key, value, TimeSpan.FromSeconds(seconds));

        return result;
    }

    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetStringValue(string key)
    {
        IDatabase db = Redis.GetDatabase();

        return db.StringGet(key);
    }

    public static void DeleteStringValue(string key)
    {
        IDatabase db = Redis.GetDatabase();

        db.KeyDelete(key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static DateTime? GetExpire(string key)
    {
        IDatabase db = Redis.GetDatabase();

        return db.KeyExpireTime(key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static long GetRemainingSeconds(string key)
    {
        var expireDateTime = GetExpire(key);
        if (expireDateTime != null)
        {
            long nowTick = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
            long expireSecond = expireDateTime!.Value.Ticks / TimeSpan.TicksPerSecond;

            return (expireSecond - nowTick);
        }

        return 0;
    }
}