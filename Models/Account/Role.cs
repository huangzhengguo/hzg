using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

public class HzgRole : BaseAccount
{
    [StringLength(32)]
    [Column("name")]
    public string Name { get; set; }

    public virtual ICollection<HzgUserRole> UserRoles { get; set; }
    public virtual ICollection<HzgRoleGroup> RoleGroups { get; set; }
}