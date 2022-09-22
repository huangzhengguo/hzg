using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hzg.Models;

public class Group : BaseAccount
{
        /// <summary>
        /// 分组名称
        /// </summary>
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<RoleGroup> GroupRoles { get; set; }
}