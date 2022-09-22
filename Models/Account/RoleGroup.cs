using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

public class RoleGroup
{
    [Column("role_id")]
    public Guid RoleId { get; set; }

    [Column("group_id")]
    public Guid GroupId { get; set; }

    public virtual Role Role { get; set; }
    public virtual Group Group { get; set; }
}