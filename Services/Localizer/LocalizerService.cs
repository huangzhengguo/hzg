using Microsoft.AspNetCore.Http;
using Hzg.Consts;

namespace Hzg.Services;

/// <summary>
/// 国际化
/// </summary>
public class LocalizerService : ILocalizerService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public LocalizerService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 多语言
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Localizer(string key)
    {
        // 获取请求头中的 Language
        var language = _httpContextAccessor.HttpContext.Request.Headers["Language"];
        if (language.Equals("en") == true)
        {
            return ErrorMessage.Messages[key];
        }

        return ErrorMessage.Messages[key];
    }

    /// <summary>
    /// 根据语言标记获取字符串
    /// </summary>
    /// <param name="language"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Localizer(string language, string key)
    {
        return ErrorMessage.Messages[key];
    }
}