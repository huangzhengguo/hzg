using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hzg.Tool;

/// <summary>
/// Json 序列化工具
/// </summary>
public static class JsonSerializerTool
{
    /// <summary>
    /// 默认序列化，JsonSerializer 静态类不能扩展，所以这里使用静态方法
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string SerializeDefault<TValue>(TValue value, bool isDate = true)
    {
        return JsonSerializer.Serialize(value, JsonSerializerTool.DefaultOptions(isDate));
    }

    /// <summary>
    /// 获取序列化选项
    /// </summary>
    /// <param name="isDate"></param>
    /// <returns></returns>
    public static JsonSerializerOptions DefaultOptions(bool isDate = true)
    {
        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        if (isDate == true)
        {
            // 使用自定义的日期格式化 YYYY-MM-DD
            options.Converters.Add(new DatetimeJsonConverter());
        }

        return options;
    }
}