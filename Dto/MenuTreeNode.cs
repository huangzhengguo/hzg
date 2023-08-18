namespace Hzg.Dto;

/// <summary>
/// 菜单目录节点
/// </summary>
public class MenuTreeNode
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
    public Guid? ParentMenuId { get; set; }

    /// <summary>
    /// 节点文本
    /// </summary>
    /// <value></value>
    public string Label { get; set; }

    /// <summary>
    /// 是否是叶子节点
    /// </summary>
    /// <value></value>
    public bool IsLeaf { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    /// <value></value>
    public MenuTreeNode[] Children { get; set; }

    /// <summary>
    /// URL
    /// </summary>
    /// <value></value>
    public string Url { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    /// <value></value>
    public string Name { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    /// <value></value>
    public string Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    /// <value></value>
    public string ComponentPath { get; set; }

    /// <summary>
    /// 元数据
    /// </summary>
    /// <value></value>
    public object Meta { get; set; }
}