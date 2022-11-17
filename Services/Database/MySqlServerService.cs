using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hzg.Services;

/// <summary>
/// 数据库服务扩展
/// </summary>
public static class MySqlServerServiceExtesion
{
    private static MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 22));
    /// <summary>
    /// 添加 MySqlServer 服务
    /// </summary>
    /// <param name="services"></param>
    public static void AddMySqlService<T>(this IServiceCollection services, IConfiguration configuration, string connectStr) where T : DbContext
    {
        services.AddDbContext<T>(options =>
        {
            options.UseMySql(configuration.GetConnectionString(connectStr), serverVersion);
        });
    }
}