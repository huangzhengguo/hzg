namespace Hzg.Const;

/// <summary>
/// 文件路径
/// </summary>
public static class FilePath
{
    private static readonly string FILE_SERVER = "http://localhost:9003/";
    private static readonly string FILE_ROOT_PATH = "wwwroot/";
    // 用户头像路径
    public static readonly string USER_AVATAR_PATH = "userfile/avatar/";
    // 用户反馈图片路径
    public static readonly string USER_FEEDBACK_PICTURE_PATH = "user/feedback/";
    // Radians产品
    public static readonly string RADIANS_PRODUCT_PATH = "product/product";
    public static readonly string RADIANS_PRODUCT_CLASSIFY_PATH = "product/productclassify";
    public static readonly string RADIANS_PRODUCT_CAROUSEL_PATH = "product/productcarousel";
    // 物联网产品
    public static readonly string IOT_PRODUCT_PICTURE_PATH = "iotproduct/product/picture";
    public static readonly string IOT_PRODUCT_ICON_PATH = "iotproduct/product/icon";
    // 产品说明书
    public static readonly string RADIANS_INSTRUCTION_PRODUCT_PATH = "instruction/product/";

    /// <summary>
    /// 文件绝对路径
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string FullFilePath(string fileName, string filePath)
    {
        return string.IsNullOrWhiteSpace(fileName) == false ? (Path.Combine(FILE_SERVER, filePath, fileName)) : null;
    }

    /// <summary>
    /// 文件相对路径
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string RelativeFilePath(string filePath)
    {
        return Path.Combine(FILE_ROOT_PATH, filePath);
    }
}