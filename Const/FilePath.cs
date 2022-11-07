using System.Text;
using System.Diagnostics;

namespace Hzg.Const;

/// <summary>
/// 文件路径
/// </summary>
public static class FilePath
{
    // 亚马逊产品
    public static readonly string RADIANS_PRODUCT_PATH = "file/product/product";
    public static readonly string RADIANS_PRODUCT_CLASSIFY_PATH = "file/product/productclassify";
    public static readonly string RADIANS_PRODUCT_CAROUSEL_PATH = "file/product/productcarousel";

    // 物联网产品
    public static readonly string IOT_MAIN_PRODUCT_PATH = "file/iot/product/mainproduct";
    public static readonly string IOT_MAIN_PRODUCT_ICON_PATH = "file/iot/product/mainproduct/icon";
    public static readonly string IOT_SUB_PRODUCT_PATH = "file/iot/product/subproduct";
    public static readonly string IOT_SUB_PRODUCT_ICON_PATH = "file/iot/product/subproduct/icon";

    // 用户反馈图片路径
    public static readonly string USER_FEEDBACK_PICTURE_PATH = "file/user/feedback/";
    // 产品说明书
    public static readonly string RADIANS_PRODUCT_INSTRUCTION_PATH = "file/instruction/product/";
    // 固件
    public static readonly string RADIANS_PRODUCT_FIRMWARE_PATH = "file/instruction/product/firmware/";

    /// <summary>
    /// FAQ 文件路径
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string FaqFilePath(string id)
    {
        return Path.Combine("file/faq", id);
    }

    /// <summary>
    /// 固件文件路径
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string FirmwareFilePath(string id)
    {
        return Path.Combine("file/firmware", id);
    }

    /// <summary>
    /// 说明书文件路径
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string InstructionFilePath(string id)
    {
        return Path.Combine("file/instruction", id);
    }

    private static readonly string FILE_SERVER = "http://192.168.0.108:9004";
    private static readonly string FILE_ROOT_PATH = "wwwroot/";

    /// <summary>
    /// 文件绝对路径
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string FullFilePath(string fileName, string filePath)
    {
        var absFilePath = string.IsNullOrWhiteSpace(fileName) == false ? (Path.Combine(FILE_SERVER, filePath, Uri.EscapeDataString(fileName))) : null;
        if (absFilePath == null)
        {
            return null;
        }

        Debug.WriteLine(absFilePath);

        return absFilePath;
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