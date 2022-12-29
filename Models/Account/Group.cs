using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

public class HzgGroup : BaseAccount
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

        public virtual ICollection<HzgUserGroup> UserGroups { get; set; }
        public virtual ICollection<HzgRoleGroup> GroupRoles { get; set; }
}