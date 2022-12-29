namespace Hzg.Models;

public class HzgTreeNode
{
    public string Id { get; set; }
    public string ParentMenuId { get; set; }
    public string Label { get; set; }
    public IEnumerable<HzgTreeNode> Children { get; set; }
    public bool IsLeaf { get; set; }
    public bool Disabled { get; set; }
}