using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

public class Role : BaseAccount
{
    [StringLength(32)]
    [Column("name")]
    public string Name { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<RoleGroup> RoleGroups { get; set; }
}