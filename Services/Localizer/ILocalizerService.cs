namespace Hzg.Services;

public interface ILocalizerService
{
    /// <summary>
    /// 多语言
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string Localizer(string key);
}