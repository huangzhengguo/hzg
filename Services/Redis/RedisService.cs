using StackExchange.Redis;
using Hzg.Consts;
using Microsoft.Extensions.Configuration;

namespace Hzg.Services;

public class RedisService : IRedisService
{
    private static ConnectionMultiplexer _redis;
    public ConnectionMultiplexer Redis
    {
        get {
            if (_redis == null)
            {
                var options = new ConfigurationOptions
                {
                    EndPoints = { _configuration["redis:server"] }
                };

                options.Password = _configuration["redis:password"];

                _redis = ConnectionMultiplexer.Connect(options);
            }

            return _redis;
        }
    }

    private readonly IConfiguration _configuration;

    public RedisService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 设置字符串
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool SetStringValue(string key, string value, long seconds)
    {
        ConnectionMultiplexer redis = this.Redis;
        IDatabase db = redis.GetDatabase();

        var result = db.StringSet(key, value, TimeSpan.FromSeconds(seconds));

        return result;
    }

    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetStringValue(string key)
    {
        IDatabase db = Redis.GetDatabase();

        return db.StringGet(key);
    }

    /// <summary>
    /// 删除值
    /// </summary>
    /// <param name="key"></param>
    public void DeleteStringValue(string key)
    {
        IDatabase db = Redis.GetDatabase();

        db.KeyDelete(key);
    }

    /// <summary>
    /// 获取过期时间
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public DateTime? GetExpire(string key)
    {
        IDatabase db = Redis.GetDatabase();

        return db.KeyExpireTime(key);
    }

    /// <summary>
    /// 获取剩余过期剩余妙数
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public long GetRemainingSeconds(string key)
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