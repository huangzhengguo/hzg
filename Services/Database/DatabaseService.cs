using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hzg.Services;

/// <summary>
/// 数据库服务扩展
/// </summary>
public static class DatabaseServiceExtesion
{
    /// <summary>
    /// 添加 MySql 服务
    /// </summary>
    /// <param name="services">服务</param>
    /// <param name="configuration">配置</param>
    /// <param name="connectStr">连接字符串</param>
    /// <typeparam name="T">数据库上下文类型</typeparam>
    public static void AddMySqlService<T>(this IServiceCollection services, IConfiguration configuration, string connectStr) where T : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectStr);
        services.AddDbContext<T>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
    }

    /// <summary>
    /// 添加 SqlServer 服务
    /// </summary>
    /// <param name="services">服务</param>
    /// <param name="configuration">配置</param>
    /// <param name="connectStr">连接字符串</param>
    /// <typeparam name="T">数据库上下文类型</typeparam>
    // static public void AddSqlServerService<T>(this IServiceCollection services, IConfiguration configuration, string connectStr) where T : DbContext
    // {
    //     services.AddDbContext<T>(options =>
    //     {
    //         options.UseSqlServer(configuration.GetConnectionString(connectStr));
    //     });
    // }
}