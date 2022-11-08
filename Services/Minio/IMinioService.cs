using Microsoft.AspNetCore.Http;

namespace Hzg.Services;

/// <summary>
/// Minio 服务
/// </summary>
public interface IMinioService
{
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="bucketName">桶</param>
    /// <param name="filePath">相对路径</param>
    /// <param name="formFile">文件数据</param>
    /// <param name="autoName">自动生成文件名</param>
    /// <returns></returns>
    Task<(bool success, string bucketName, string objectName, string fileName)> UploadFile(string bucketName, string filePath, IFormFile formFile, bool autoName = false);

    /// <summary>
    /// 上传文件到默认桶
    /// </summary>
    /// <param name="filePath">相对路径</param>
    /// <param name="formFile">文件数据</param>
    /// <param name="autoName">自动生成文件名</param>
    /// <returns></returns>
    // Task<(bool success, string bucketName, string objectName, string fileName)> UploadFileToDefaultBucket(string filePath, IFormFile formFile, bool autoName = false);

    /// <summary>
    /// 获取文件 URL
    /// </summary>
    /// <param name="bucketName">桶</param>
    /// <param name="objectName">对象名</param>
    /// <param name="contentType">内容类型</param>
    /// <returns></returns>
    Task<string> GetFileUrl(string bucketName, string objectName, string contentType = "");

    /// <summary>
    /// 获取文件 URL
    /// </summary>
    /// <param name="bucketName">桶</param>
    /// <param name="objectName">对象名</param>
    /// <param name="contentType">内容类型</param>
    /// <returns></returns>
    // Task<string> GetDefaultFileUrl(string objectName, string contentType = "");

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="bucketName">桶</param>
    /// <param name="objectName">文件</param>
    /// <returns></returns>
    Task<bool> RemoveFile(string bucketName, string objectName);

    /// <summary>
    /// 删除默认桶的文件
    /// </summary>
    /// <param name="objectName">文件</param>
    /// <returns></returns>
    // Task<bool> RemoveDefaultBucketFile(string objectName);

    /// <summary>
    /// 更新文件到桶
    /// </summary>
    /// <param name="formFile">文件数据</param>
    /// <param name="filePath">文件路径</param>
    /// <param name="newFileName">新文件名</param>
    /// <param name="oldFileName">原文件名</param>
    /// <param name="autoName">是否自动生成文件名</param>
    /// <returns>文件名称</returns>
    Task<string> UpdateFile(string bucketName, IFormFile formFile, string filePath, string newFileName, string oldFileName, bool autoName = false);

    /// <summary>
    /// 更新文件到默认桶
    /// </summary>
    /// <param name="formFile">文件数据</param>
    /// <param name="filePath">文件路径</param>
    /// <param name="newFileName">新文件名</param>
    /// <param name="oldFileName">原文件名</param>
    /// <param name="autoName">是否自动生成文件名</param>
    /// <returns>文件名称</returns>
    // Task<string> UpdateFileToDefaultBucket(IFormFile formFile, string filePath, string newFileName, string oldFileName, bool autoName = false);
}