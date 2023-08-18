namespace Hzg.Consts;

/// <summary>
/// 触发条件
/// </summary>
public enum TriggerConditionEnum : UInt16
{
    /// <summary>
    /// 开关联动
    /// </summary>
    On = 0,
    /// <summary>
    /// 传感器联动
    /// </summary>
    Off = 1,
    /// <summary>
    /// 低于
    /// </summary>
    LessThan = 2,
    /// <summary>
    /// 高于
    /// </summary>
    MoreThan = 3
}