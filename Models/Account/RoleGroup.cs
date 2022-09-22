namespace Hzg.Models;

public class RoleGroup
{
    public Guid RoleId { get; set; }
    public Guid GroupId { get; set; }

    public virtual Role Role { get; set; }
    public virtual Group Group { get; set; }
}