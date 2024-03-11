using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Hzg.Services;

/// <summary>
/// JWT 扩展
/// </summary>
public static class JwtServiceExtension
{
    /// <summary>
    /// JWT 服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns></returns>
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IJwtService, JwtService>();

        var issuer = configuration[JwtOptionsConst.IssuerSettingPath];
        var audience = configuration[JwtOptionsConst.AudienceSettingPath];
        var security = configuration[JwtOptionsConst.SecurityKeySettingPath];

        var key = Encoding.ASCII.GetBytes(security);

        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                // ClockSkew = TimeSpan.Zero,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            // options.TokenHandlers.Add(new BearerTokenHandler());
            // options.Events = new JwtBearerEvents()
            // {
            //     OnAuthenticationFailed = c =>
            //     {
            //         return c.Response.WriteAsJsonAsync("认证出错");
            //     },
            //     OnTokenValidated = c =>
            //     {
            //         return Task.CompletedTask;
            //     }
            // };
        });

        return services;
    }
}