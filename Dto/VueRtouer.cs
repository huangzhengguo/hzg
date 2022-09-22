namespace Hzg.Dto;

public class VueRouter
{
    public string Name { get; set; }
    public string Path { get; set; }
    public object Meta { get; set; }

    public VueRouter[] Children { get; set; } 
}