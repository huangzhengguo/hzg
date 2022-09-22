using Yitter.IdGenerator;

namespace Hzg.Tool;

public class IdGeneratorTool
{
    private readonly static IdGeneratorOptions options = new IdGeneratorOptions()
    {
        WorkerId = 10
    };
    private static IdGeneratorTool idGeneratorTool = null;

    public static IdGeneratorTool Tool
    {
        get
        {
            if (idGeneratorTool == null)
            {
                idGeneratorTool = new IdGeneratorTool();

                YitIdHelper.SetIdGenerator(options);
            }

            return idGeneratorTool;
        }
    }

    /// <summary>
    /// 生成id
    /// </summary>
    /// <returns></returns>
    public long GetnerateId()
    {
        return YitIdHelper.NextId();
    }
}