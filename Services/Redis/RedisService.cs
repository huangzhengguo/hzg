using StackExchange.Redis;
using Hzg.Consts;
using Microsoft.Extensions.Configuration;

namespace Hzg.Services;

/// <summary>
/// Redis 服务
/// </summary>
public class RedisService : IRedisService
{
    private static ConnectionMultiplexer _redis;

    /// <summary>
    /// Redis 实例
    /// </summary>
    /// <value></value>
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

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="configuration"></param>
    public RedisService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 设置字符串
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="seconds">有效期秒数</param>
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