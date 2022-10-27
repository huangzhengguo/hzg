namespace Hzg.Const;

/// <summary>
/// 触发条件
/// </summary>
public enum TriggerConditionEnum : UInt16
{
    // 开关联动
    On = 0,
    // 传感器联动
    Off = 1,
    // 低于
    LessThan = 2,
    // 高于
    MoreThan = 3
}