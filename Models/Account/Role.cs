namespace Hzg.Models;

public class Role : BaseAccount
{
    public string Name { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<RoleGroup> RoleGroups { get; set; }
}