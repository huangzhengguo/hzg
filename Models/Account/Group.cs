using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

public class Group : BaseAccount
{
        /// <summary>
        /// 分组名称
        /// </summary>
        [Required]
        [StringLength(256)]
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        [Column("description")]
        public string Description { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<RoleGroup> GroupRoles { get; set; }
}