using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

public class UserRole
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("role_id")]
    public Guid RoleId { get; set; }

    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}