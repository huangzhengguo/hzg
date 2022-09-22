using System;
using System.Reflection;

namespace Hzg.Tool;

public static class CommonTool
{
    /// <summary>
    /// 复制一个对象的属性到另一个对象
    /// </summary>
    /// <param name="s"></param>
    /// <param name="t"></param>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
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
                propertyInfo.SetValue(t, value, null);
            }
        }
    }
}