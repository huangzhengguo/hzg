using Microsoft.EntityFrameworkCore;

namespace Hzg.Tool;

/// <summary>
/// 分页列表
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class PagedList<T> : List<T>
{
    
    /// <summary>
    /// 当前页索引
    /// </summary>
    /// <value></value>
    public int PageIndex { get; private set; }

    /// <summary>
    /// 总页数
    /// </summary>
    /// <value></value>
    public int TotalPages { get; private set; }

    /// <summary>
    /// 记录总数
    /// </summary>
    /// <value></value>
    public int AllDataCount { get; private set; }

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="items">项目列表</param>
    /// <param name="count">数量</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    public PagedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AllDataCount = count;

        this.AddRange(items);
    }

    /// <summary>
    /// 是否是第一页
    /// </summary>
    /// <value></value>
    public bool IsFirstPage
    {
        get
        {
            return (PageIndex <= 1);
        }
    }

    /// <summary>
    /// 是否是最后一页
    /// </summary>
    /// <value></value>
    public bool IsEndPage
    {
        get
        {
            return (PageIndex >= TotalPages);
        }
    }

    /// <summary>
    /// 分页列表数据
    /// </summary>
    /// <param name="source">数据源</param>
    /// <param name="pageIndex">页索引</param>
    /// <param name="pageSize">每页数据个数</param>
    /// <returns>列表数据</returns>
    public static async Task<PagedList<T>> ListAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        // 使用此方法，而不使用构造方法，是因为构造方法不能异步运行
        var count = await source.CountAsync();

        var items = await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, count, pageIndex, pageSize);
    }
}