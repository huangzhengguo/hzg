using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Minio;
using Hzg.Tool;
using Hzg.Consts;

namespace Hzg.Services;

public class MinioService : IMinioService
{
    private static string DefaultBucketName = "radians";
    private static MinioClient _minioClient;
    private readonly IConfiguration _configuration;

    public MinioService(IConfiguration configuration)
    {
        _configuration = configuration;

        var endpoint = _configuration["minio:endpoint"];
        var accessKey = _configuration["minio:accessKey"];
        var secretKey = _configuration["minio:secretKey"];
        bool secure = Convert.ToBoolean(_configuration["minio:secure"]);

        if (_minioClient == null)
        {
            var con = new MinioConfiguration()
            {
                Endpoint = endpoint,
                AccessKey = accessKey,
                SecretKey = secretKey,
                Secure = secure
            };

            _minioClient = MinioTool.CreateMinioClient(con);
        }
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="bucketName">桶</param>
    /// <param name="filePath">相对路径</param>
    /// <param name="formFile">文件数据</param>
    /// <param name="autoName">是否自动生成文件名</param>
    /// <returns></returns>
    public async Task<(bool success, string bucketName, string objectName, string fileName)> UploadFile(string bucketName, string filePath, IFormFile formFile, bool autoName = false)
    {
        if (formFile == null || formFile.Length == 0 || formFile.FileName == null)
        {
            // 如果文件为空，返回成功
            return (true, null, null, null);
        }

        var fileName = Guid.NewGuid() + formFile.FileName.Substring(formFile.FileName.LastIndexOf("."));
        if (autoName == false)
        {
            // 保留原文件名
            fileName = formFile.FileName;
        }
        var objectName = Path.Combine(filePath, fileName);
        var stream = formFile.OpenReadStream();
        var success = await MinioTool.UploadFile(_minioClient, bucketName, objectName, stream, formFile.Length, formFile.ContentType);
        if (success == true)
        {
            return (true, bucketName, objectName, fileName);
        }

        return (false, null, null, null);
    }

    /// <summary>
    /// 上传文件到默认桶
    /// </summary>
    /// <param name="filePath">相对路径</param>
    /// <param name="formFile">文件数据</param>
    /// <param name="autoName">自动生成文件名</param>
    /// <returns></returns>
    // public async Task<(bool success, string bucketName, string objectName, string fileName)> UploadFileToDefaultBucket(string filePath, IFormFile formFile, bool autoName = false)
    // {
    //     return await this.UploadFile(RadiansConst.DefaultBucketName, filePath, formFile, autoName);
    // }

    /// <summary>
    /// 获取文件 URL
    /// </summary>
    /// <param name="bucketName">桶</param>
    /// <param name="objectName">对象名</param>
    /// <param name="contentType">内容类型</param>
    /// <returns></returns>
    public async Task<string> GetFileUrl(string bucketName, string objectName, string contentType = "")
    {
        return await MinioTool.GetFileUrl(_minioClient, bucketName, objectName, contentType);
    }

    /// <summary>
    /// 获取文件 URL
    /// </summary>
    /// <param name="bucketName">桶</param>
    /// <param name="objectName">对象名</param>
    /// <param name="contentType">内容类型</param>
    /// <returns></returns>
    // public async Task<string> GetDefaultFileUrl(string objectName, string contentType)
    // {
    //     return await GetFileUrl(RadiansConst.DefaultBucketName, objectName, contentType);
    // }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="bucketName">桶</param>
    /// <param name="objectName">文件</param>
    /// <returns></returns>
    public async Task<bool> RemoveFile(string bucketName, string objectName)
    {
        return await MinioTool.RemoveFile(_minioClient, bucketName, objectName);
    }

    /// <summary>
    /// 删除默认桶的文件
    /// </summary>
    /// <param name="objectName">文件</param>
    /// <returns></returns>
    // public async Task<bool> RemoveDefaultBucketFile(string objectName)
    // {
    //     return await MinioTool.RemoveFile(_minioClient, RadiansConst.DefaultBucketName, objectName);
    // }

    /// <summary>
    /// 更新文件到桶
    /// </summary>
    /// <param name="formFile">文件数据</param>
    /// <param name="filePath">文件路径</param>
    /// <param name="newFileName">新文件名</param>
    /// <param name="oldFileName">原文件名</param>
    /// <param name="autoName">是否自动生成文件名</param>
    /// <returns>文件名称</returns>
    public async Task<string> UpdateFile(string bucketName, IFormFile formFile, string filePath, string newFileName, string oldFileName, bool autoName = false)
    {
        var fileName = "";
        if (string.IsNullOrWhiteSpace(newFileName) == true)
        {
            // 删除图片
            fileName = null;
        }
        else if (string.IsNullOrWhiteSpace(newFileName) == false && formFile == null)
        {
            // 保持不变
            fileName = oldFileName;
        }
        else
        {
            // 更新图片
            var result = await this.UploadFile(bucketName, filePath, formFile, autoName);

            fileName = result.fileName;
        }

        return fileName;
    }

    /// <summary>
    /// 更新文件到默认桶
    /// </summary>
    /// <param name="formFile">文件数据</param>
    /// <param name="filePath">文件路径</param>
    /// <param name="newFileName">新文件名</param>
    /// <param name="oldFileName">原文件名</param>
    /// <param name="autoName">是否自动生成文件名</param>
    // /// <returns>文件名称</returns>
    // public async Task<string> UpdateFileToDefaultBucket(IFormFile formFile, string filePath, string newFileName, string oldFileName, bool autoName = false)
    // {
    //     var fileName = "";
    //     if (string.IsNullOrWhiteSpace(newFileName) == true)
    //     {
    //         // 删除图片
    //         fileName = null;
    //     }
    //     else if (string.IsNullOrWhiteSpace(newFileName) == false && formFile == null)
    //     {
    //         // 保持不变
    //         fileName = oldFileName;
    //     }
    //     else
    //     {
    //         // 更新图片
    //         var result = await this.UploadFile(RadiansConst.DefaultBucketName, filePath, formFile, autoName);

    //         fileName = result.fileName;
    //     }

    //     return fileName;
    // }
}