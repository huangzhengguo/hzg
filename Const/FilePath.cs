namespace Hzg.Const;

/// <summary>
/// 文件路径
/// </summary>
public static class FilePath
{
    private static readonly string FILE_SERVER = "http://ik62307818.goho.co:9004/";
    private static readonly string FILE_ROOT_PATH = "wwwroot/";
    // 用户头像路径
    public static readonly string USER_AVATAR_PATH = "file/userfile/avatar/";
    // 用户反馈图片路径
    public static readonly string USER_FEEDBACK_PICTURE_PATH = "file/user/feedback/";
    // Radians产品
    public static readonly string RADIANS_PRODUCT_PATH = "file/product/product";
    public static readonly string RADIANS_PRODUCT_CLASSIFY_PATH = "file/product/productclassify";
    public static readonly string RADIANS_PRODUCT_CAROUSEL_PATH = "file/product/productcarousel";
    // 物联网产品
    public static readonly string IOT_PRODUCT_PICTURE_PATH = "file/iotproduct/product/picture";
    public static readonly string IOT_PRODUCT_ICON_PATH = "file/iotproduct/product/icon";
    // 产品说明书
    public static readonly string RADIANS_PRODUCT_INSTRUCTION_PATH = "file/instruction/product/";
    // FAQ
    public static readonly string RADIANS_PRODUCT_FAQ_PATH = "file/instruction/product/";
    // 固件
    public static readonly string RADIANS_PRODUCT_FIRMWARE_PATH = "file/instruction/product/firmware/";

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