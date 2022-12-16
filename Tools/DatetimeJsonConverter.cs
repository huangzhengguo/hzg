using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hzg.Tool;

/// <summary>
/// 处理 Json 格式化日期
/// </summary>
public class DatetimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString());
    }

    // public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    // {
    //     Debug.Assert(typeToConvert == typeof(DateTimeOffset));
    //     return DateTimeOffset.Parse(reader.GetString());
    // }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}