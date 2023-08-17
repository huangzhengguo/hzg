using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hzg.Models;

/// <summary>
/// 分组
/// </summary>
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

        /// <summary>
        /// 用户分组
        /// </summary>
        /// <value></value>
        public virtual ICollection<HzgUserGroup> UserGroups { get; set; }

        /// <summary>
        /// 分组角色
        /// </summary>
        /// <value></value>
        public virtual ICollection<HzgRoleGroup> GroupRoles { get; set; }
}