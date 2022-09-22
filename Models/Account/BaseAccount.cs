namespace Hzg.Models;

/// <summary>
/// 账号基类
/// </summary>
public abstract class BaseAccount<KeyT>
{
    /// <summary>
    /// 主键
    /// </summary>
    /// <value></value>
    public KeyT Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateDateTime { get; set; }

    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? UpdateDateTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <value></value>
    public string Remark { get; set; }
}

public abstract class BaseAccount : BaseEntity<Guid>
{}