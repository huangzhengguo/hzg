using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

public class UserGroup
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("group_id")]
    public Guid GroupId { get; set; }

    public virtual User User { get; set; }
    public virtual Group Group { get; set; }
}