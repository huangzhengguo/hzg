using Microsoft.EntityFrameworkCore;
using Hzg.Dto;
using Hzg.Models;
using Hzg.Data;

namespace Hzg.Tool;

/// <summary>
/// 分组目录树
/// </summary>
public class GroupTreeNode
{
    /// <summary>
    /// 标识
    /// </summary>
    /// <value></value>
    public Guid? Id { get; set; }

    /// <summary>
    /// 父节点标识
    /// </summary>
    /// <value></value>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    /// <value></value>
    public string Title { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    /// <value></value>
    public List<GroupTreeNode> Children { get; set; } 
}

/// <summary>
/// 分组工具类
/// </summary>
public class GroupTool
{
    /// <summary>
    /// 生成前端目录树数据
    /// </summary>
    /// <param name="data">分组数据</param>
    /// <param name="group">父分组</param>
    /// <returns></returns>
    public static List<GroupTreeNode> GenerateTreeData(List<HzgGroup> data, HzgGroup group)
    {
        var resultJson = new List<GroupTreeNode>();

        List<HzgGroup> childrenData;
        if (group == null)
        {
            // 根节点
            childrenData = data.Where(m => m.ParentId == null).ToList();
            if (childrenData.Count == 0 || childrenData == null)
            {
                return resultJson;
            }

            var rootList = new List<GroupTreeNode>();
            foreach(var item in childrenData)
            {
                rootList.AddRange(GenerateTreeData(data, item));
            }

            return rootList;
        }

        childrenData = data.Where(m => m.ParentId == group.Id).ToList();
        // 非根节点
        var rootJson = new GroupTreeNode
        {
            Id = group.Id,
            ParentId = group.ParentId,
            Title = group.Name
        };

        var childrenList = new List<GroupTreeNode>();
        foreach(var item in childrenData)
        {
            childrenList.AddRange(GenerateTreeData(data, item));
        }
        
        rootJson.Children = childrenList.ToList();

        resultJson.Add(rootJson);

        return resultJson;
    }
}