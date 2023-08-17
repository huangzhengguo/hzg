namespace Hzg.Services;

/// <summary>
/// 国际化
/// </summary>
public interface ILocalizerService
{
    /// <summary>
    /// 多语言
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string Localizer(string key);
}