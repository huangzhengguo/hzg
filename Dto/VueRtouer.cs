namespace Hzg.Dto;

/// <summary>
/// 前端 Vue 路由节点
/// </summary>
public class VueRouter
{
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
    /// 元数据
    /// </summary>
    /// <value></value>
    public object Meta { get; set; }

    /// <summary>
    /// 子路由
    /// </summary>
    /// <value></value>
    public VueRouter[] Children { get; set; } 
}