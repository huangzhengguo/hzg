using Microsoft.AspNetCore.Http;
using Hzg.Const;

namespace Hzg.Tool;

public static class FileManagerTool
{
    /// <summary>
    /// 上传单个文件
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static async Task<string> UploadFile(IFormFile formFile, string filePath, string fileName = null)
    {
        if (formFile == null)
        {
            return null;
        }

        if (formFile.Length > 0)
        {
            var fullFilePath = Path.Combine(filePath);
            
            // 检测目录是否存在
            if (Directory.Exists(fullFilePath) == false)
            {
                Directory.CreateDirectory(fullFilePath);
            }

            // 自动生成文件名
            var newFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + formFile.FileName;
            if (newFileName.Length > 64)
            {
                // 文件名不能太长
                newFileName = newFileName.Substring(0, 36) + newFileName.Substring(newFileName.Length - 20);
            }
            var fullFilePathName = Path.Combine(fullFilePath, newFileName);
            using(var stream = System.IO.File.Create(fullFilePathName))
            {
                await formFile.CopyToAsync(stream);
            }

            return newFileName;
        }

        return null;
    }

    /// <summary>
    /// 更新文件
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="filePath"></param>
    /// <param name="newFileName"></param>
    /// <param name="oldFileName"></param>
    /// <returns>文件名称</returns>
    public static async Task<string> UpdateFile(IFormFile formFile, string filePath, string newFileName, string oldFileName)
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
            fileName = await FileManagerTool.UploadFile(formFile, filePath);
        }

        return fileName;
    }

}