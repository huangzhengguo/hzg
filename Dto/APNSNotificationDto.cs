using System.ComponentModel.DataAnnotations;

namespace Hzg.Dto;

/// <summary>
/// 苹果推送数据
/// </summary>
public class APNSNotificationDto
{
    /// <summary>
    /// 标题
    /// </summary>
    /// <value></value>
    public string Title { get; set; }

    /// <summary>
    /// 子标题
    /// </summary>
    /// <value></value>
    public string Subtitle { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    /// <value></value>
    public string Body { get; set; }

    /// <summary>
    /// 自定义数据
    /// </summary>
    /// <value></value>
    public IReadOnlyDictionary<string, string> Data { get; set; }
}