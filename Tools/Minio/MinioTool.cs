using System.Diagnostics;
using System.Security.Cryptography;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;

namespace Hzg.Tool;

/// <summary>
/// Minio 工具类
/// </summary>
public static class HZG_MinioTool
{
    /// <summary>
    /// 创建 Minio 客户端
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <returns></returns>
    public static MinioClient CreateMinioClient(MinioConfiguration configuration)
    {
        return new MinioClient()
                            .WithEndpoint(configuration.Endpoint)
                            .WithCredentials(configuration.AccessKey, configuration.SecretKey)
                            .WithSSL(configuration.Secure)
                            .Build();
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="minioClient"></param>
    /// <param name="bucketName"></param>
    /// <param name="objectName"></param>
    /// <param name="stream"></param>
    /// <param name="size"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static async Task<bool> UploadFile(MinioClient minioClient, string bucketName, string objectName, Stream stream, long size, string type)
    {
        try
        {
            // 如果服务器上没有 bucket ，则创建一个
            var bucketArgs = new BucketExistsArgs().WithBucket(bucketName);
            bool isExist = await minioClient.BucketExistsAsync(bucketArgs).ConfigureAwait(false);
            if (!isExist)
            {
                // 新建 bucket
                var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
                await minioClient.MakeBucketAsync(makeBucketArgs).ConfigureAwait(false);
            }

            // 上传文件到 bucket
            var putObjectArgs = new PutObjectArgs()
                                .WithBucket(bucketName)
                                .WithObject(objectName)
                                .WithStreamData(stream)
                                .WithObjectSize(size)
                                .WithContentType(type);

            await minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

            return true;
        }
        catch(MinioException e)
        {
            Debug.WriteLine("UploadFile Error: " + e.Message);
        }

        return false;
    }

    /// <summary>
    /// 获取 URL
    /// </summary>
    /// <param name="minioClient">Minio 客户端</param>
    /// <param name="bucketName">桶</param>
    /// <param name="objectName">对象</param>
    /// <param name="contentType">内容类型</param>
    /// <param name="expire">过期时长</param>
    /// <returns></returns>
    public static async Task<string> GetFileUrl(MinioClient minioClient, string bucketName, string objectName, string contentType, int expire = 60 * 60 * 24)
    {
        try
        {
            PresignedGetObjectArgs args = new PresignedGetObjectArgs()
                                            .WithBucket(bucketName)
                                            .WithObject(objectName)
                                            .WithExpiry(expire);

            Debug.WriteLine("contentType = " + contentType);
            string url = await minioClient.PresignedGetObjectAsync(args);
            
            return url;
        }
        catch (MinioException e)
        {
            Debug.WriteLine("GetFileUrl Error: " + e.Message);
        }

        return null;
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="minioClient">Minio 客户端</param>
    /// <param name="bucketName">桶</param>
    /// <param name="objectName">对象</param>
    /// <returns></returns>
    public static async Task<bool> RemoveFile(MinioClient minioClient, string bucketName, string objectName)
    {
        try
        {
            RemoveObjectArgs removeObjectArgs = new RemoveObjectArgs()
                                                .WithBucket(bucketName)
                                                .WithObject(objectName);

            await minioClient.RemoveObjectAsync(removeObjectArgs);

            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine("RemoveFile Error: " + e.Message);
        }

        return false;
    }

    // public static async Task<string> CopyObjceAsync(MinioClient minioClient, string bucketName, string objectName, string dstBucketName, string dstObjectName)
    // {
    //     try
    //     {
    //         CopySourceObjectArgs copySourceObjectArgs = new CopySourceObjectArgs()
    //                                                     .wi
    //         CopyObjectArgs copyObjectArgs = new CopyObjectArgs()
    //                                         .WithCopyObjectSource();



    //         CopyConditions copyConditions = new CopyConditions();

    //         copyConditions.SetMatchETag("CopyETag");

    //         ServerSideEncryption sseSrc, sseDst;

    //         Aes aesEncryption = Aes.Create();
    //         aesEncryption.KeySize = 256;
    //         aesEncryption.GenerateKey();
    //         sseSrc = new SSEC(aesEncryption.Key);
    //         sseDst = new SSES3();

    //         await minioClient.CopyObjectAsync(bucketName, objectName, dstBucketName, dstObjectName, sseSrc: sseSrc, sseDest: sseDst);
    //     }
    // }
}