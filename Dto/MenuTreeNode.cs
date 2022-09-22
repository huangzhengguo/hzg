namespace Hzg.Dto;

public class MenuTreeNode
{
    public Guid? Id { get; set; }
    public Guid? ParentMenuId { get; set; }
    public string Label { get; set; } 
    public bool IsLeaf { get; set; }
    public MenuTreeNode[] Children { get; set; } 
    public string Url { get; set; }

    public string Name { get; set; }
    public string Path { get; set; }
    public string ComponentPath { get; set; }
    public object Meta { get; set; }
}