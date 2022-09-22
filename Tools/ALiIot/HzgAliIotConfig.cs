
using Microsoft.Extensions.Configuration;

namespace Hzg.Tool;

/// <summary>
/// 阿里云物联网配置信息
/// </summary>
public class HzgAliIotConfig
{
    public HzgAliIotConfig(IConfiguration configuration)
    {
        this.Config = new AlibabaCloud.OpenApiClient.Models.Config();

        this.Config.AccessKeyId = configuration["aliIot:accessKey"];
        this.Config.AccessKeySecret = configuration["aliIot:accessSecret"];
        this.Config.RegionId = configuration["aliIot:regionId"];


        // this.AccessKey = configuration["aliIot:accessKey"];
        // this.AccessSecret = configuration["aliIot:accessSecret"];
        // this.Uid = configuration["aliIot:uid"];
        // this.RegionId = configuration["aliIot:regionId"];
        // this.ConsumerGroupId = configuration["aliIot:consumerGroupId"];
    }

    public AlibabaCloud.OpenApiClient.Models.Config Config { get; }

    // /// <summary>
    // /// 阿里云accessKey
    // /// </summary>
    // /// <value></value>
    // public String AccessKey { get; }

    // /// <summary>
    // /// 阿里云accessSecret
    // /// </summary>
    // /// <value></value>
    // public String AccessSecret { get; }

    // /// <summary>
    // /// 阿里云uid
    // /// </summary>
    // /// <value></value>
    // public String Uid { get; }

    // /// <summary>
    // /// 地区regionId
    // /// </summary>
    // /// <value></value>
    // public String RegionId { get; }

    // /// <summary>
    // /// 消费组ID 请在控制台上查看您的服务端订阅消费组ID
    // /// </summary>
    // /// <value></value>
    // public String ConsumerGroupId { get; }
}
