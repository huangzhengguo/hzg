using Microsoft.AspNetCore.Http;

namespace Hzg.Tool;

public static class FileManagerTool
{
    /// <summary>
    /// 上传单个文件
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static async Task<string> UploadFile(IFormFile formFile, string filePath)
    {
        if (formFile.Length > 0)
        {
            var fullFilePath = Path.Combine(filePath);
            
            // 检测目录是否存在
            if (Directory.Exists(fullFilePath) == false)
            {
                Directory.CreateDirectory(fullFilePath);
            }

            // 自动生成文件名
            var fileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + formFile.FileName;
            if (fileName.Length > 64)
            {
                // 文件名不能太长
                fileName = fileName.Substring(0, 36) + fileName.Substring(fileName.Length - 20);
            }
            var fullFilePathName = Path.Combine(fullFilePath, fileName);
            using(var stream = System.IO.File.Create(fullFilePathName))
            {
                await formFile.CopyToAsync(stream);
            }

            return fileName;
        }

        return null;
    }
}