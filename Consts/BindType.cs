namespace Hzg.Consts;

/// <summary>
/// 绑定类型
/// </summary>
public enum BindType
{
    /// <summary>
    /// 管理员
    /// </summary>
    Admin = 0,
    /// <summary>
    /// 成员
    /// </summary>
    Member = 1
}

/// <summary>
/// 绑定类型和角色信息
/// </summary>
public static class BindTypeRole
{
    /// <summary>
    /// 转换绑定类型为角色
    /// </summary>
    /// <param name="bindType"></param>
    /// <returns></returns>
    public static string BindTypeToRole(BindType bindType)
    {
        return (bindType == BindType.Admin ? "管理员" : "队员");
    }
}