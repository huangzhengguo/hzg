namespace Hzg.Models;

/// <summary>
/// 目录树节点
/// </summary>
public class HzgTreeNode
{
    /// <summary>
    /// 标识
    /// </summary>
    /// <value></value>
    public string Id { get; set; }

    /// <summary>
    /// 父节点标识
    /// </summary>
    /// <value></value>
    public string ParentMenuId { get; set; }

    /// <summary>
    /// 标签文本
    /// </summary>
    /// <value></value>
    public string Label { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    /// <value></value>
    public IEnumerable<HzgTreeNode> Children { get; set; }

    /// <summary>
    /// 是否是叶子节点
    /// </summary>
    /// <value></value>
    public bool IsLeaf { get; set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    /// <value></value>
    public bool Disabled { get; set; }
}