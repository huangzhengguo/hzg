using System;
using System.Reflection;

namespace Hzg.Tool;

/// <summary>
/// 公用工具
/// </summary>
public static class HZG_CommonTool
{
    /// <summary>
    /// 复制一个对象的属性到另一个对象
    /// </summary>
    /// <param name="s"></param>
    /// <param name="t"></param>
    /// <typeparam name="ST"></typeparam>
    /// <typeparam name="DT"></typeparam>
    /// <returns></returns>
    public static void CopyProperties<ST, DT>(ST s, DT t)
    {
        var sTProperties = s.GetType().GetProperties();
        Type targetType = t.GetType();
        foreach(var sp in sTProperties)
        {
            PropertyInfo propertyInfo = targetType.GetProperty(sp.Name);
            if (propertyInfo == null)
            {
                continue;
            }

            object value = sp.GetValue(s, null);
            if (value != null)
            {
                // string 赋值给 Guid
                if (sp.PropertyType == typeof(string) && (propertyInfo.PropertyType == typeof(Guid) || propertyInfo.PropertyType == typeof(Guid?)))
                {
                    propertyInfo.SetValue(t, new Guid(value.ToString()), null);
                } else if ((sp.PropertyType == typeof(Guid) || sp.PropertyType == typeof(Guid?)) && propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(t, value?.ToString(), null);
                }
                else {
                    propertyInfo.SetValue(t, value, null);
                }
            }
        }
    }

    /// <summary>
    /// 转换
    /// </summary>
    /// <param name="kr">源</param>
    /// <typeparam name="DT">目的类型</typeparam>
    /// <typeparam name="ST">源类型</typeparam>
    /// <returns></returns>
    public static DT SelectConvertFunc<DT, ST>(ST kr) where DT : new()
    {
        var vo = new DT();

        HZG_CommonTool.CopyProperties(kr, vo);

        return vo;
    }

    /// <summary>
    /// 获取 32 位的字符串
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string GetSortCode(int count)
    {
        return count.ToString("D32");
    }
}