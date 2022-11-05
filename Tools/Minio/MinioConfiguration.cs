namespace Hzg.Tool;

/// <summary>
/// Minio 配置
/// </summary>
public class MinioConfiguration
{
    /// <summary>
    /// 服务器地址
    /// </summary>
    /// <value></value>
    public string Endpoint { get; set; }

    /// <summary>
    /// 凭证 Access Key
    /// </summary>
    /// <value></value>
    public string AccessKey { get; set; }

    /// <summary>
    /// 密钥
    /// </summary>
    /// <value></value>
    public string SecretKey { get; set; }

    /// <summary>
    /// 是否加密
    /// </summary>
    /// <value></value>
    public bool Secure { get; set; }
}