using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

public class HzgRoleGroup
{
    [Column("role_id")]
    public Guid RoleId { get; set; }

    [Column("group_id")]
    public Guid GroupId { get; set; }

    public virtual HzgRole Role { get; set; }
    public virtual HzgGroup Group { get; set; }
}