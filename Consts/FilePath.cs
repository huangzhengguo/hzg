using System.Text;
using System.Diagnostics;

namespace Hzg.Consts;

/// <summary>
/// 文件路径
/// </summary>
public static class FilePath
{
    /// <summary>
    /// 亚马逊产品
    /// </summary>
    public static readonly string RADIANS_PRODUCT_PATH = "file/product/product";

    /// <summary>
    /// 亚马逊产品分类
    /// </summary>
    public static readonly string RADIANS_PRODUCT_CLASSIFY_PATH = "file/product/productclassify";

    /// <summary>
    /// 亚马逊产品轮播图
    /// </summary>
    public static readonly string RADIANS_PRODUCT_CAROUSEL_PATH = "file/product/productcarousel";

    /// <summary>
    /// 物联网产品
    /// </summary>
    public static readonly string IOT_MAIN_PRODUCT_PATH = "file/iot/product/mainproduct";
    /// <summary>
    /// 物联网主产品
    /// </summary>
    public static readonly string IOT_MAIN_PRODUCT_ICON_PATH = "file/iot/product/mainproduct/icon";
    /// <summary>
    /// 物联网子产品
    /// </summary>
    public static readonly string IOT_SUB_PRODUCT_PATH = "file/iot/product/subproduct";
    /// <summary>
    /// 物联网子产品
    /// </summary>
    public static readonly string IOT_SUB_PRODUCT_ICON_PATH = "file/iot/product/subproduct/icon";

    /// <summary>
    /// 用户反馈图片路径
    /// </summary>
    public static readonly string USER_FEEDBACK_PICTURE_PATH = "file/user/feedback/";
    /// <summary>
    /// 产品说明书
    /// </summary>
    public static readonly string RADIANS_PRODUCT_INSTRUCTION_PATH = "file/instruction/product/";
    /// <summary>
    /// 固件
    /// </summary>
    public static readonly string RADIANS_PRODUCT_FIRMWARE_PATH = "file/instruction/product/firmware/";

    /// <summary>
    /// 用户头像路径
    /// </summary>
    public static readonly string USER_AVATAR_PATH = "file/user/avatar/";

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
}