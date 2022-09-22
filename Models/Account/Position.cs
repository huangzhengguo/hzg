namespace Hzg.Models;

public class Position : BaseEntity
{
    /// <summary>
    /// 职位名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 职位编码
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// 顺序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 状态(字典)
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }
    /// <summary>
    /// 更新者
    /// </summary>
    public long? UpdateUser { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}