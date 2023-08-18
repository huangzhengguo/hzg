using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Hzg.Data;
using Hzg.Consts;
using Hzg.Models;
using Hzg.Services;
using Hzg.Tool;

namespace Hzg.Controllers;

/// <summary>
/// 分组
/// </summary>
[ApiController]
[Authorize]
[Route("api/account/group/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HzgGroupController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private readonly IUserService _userService;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="accountContext">用户数据上下文</param>
    /// <param name="userService">用户服务</param>
    public HzgGroupController(AccountDbContext accountContext, IUserService userService)
    {
        this._accountContext = accountContext;
        this._userService = userService;
    }

    /// <summary>
    /// 获取所有分组
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get")]
    public async Task<string> get(string name)
    {
        var groups = await _accountContext.Groups.AsNoTracking().Where(g => g.Name == name).OrderBy(g => g.Name).ToListAsync();
        if (string.IsNullOrWhiteSpace(name))
        {
            groups = await _accountContext.Groups.AsNoTracking().OrderBy(g => g.Name).ToListAsync();
        }

        var response = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = groups
        };

        return JsonSerializerTool.SerializeDefault(response);
    }

    /// <summary>
    /// 新建分组
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<string> Create([FromBody] HzgGroup group)
    {
        var model = new HzgGroup();

        model.Name = group.Name;

        var response = new ResponseData()
        {
            ShowMsg = true,
            Code = ErrorCode.Group_Has_Exist
        };

        var g = await _accountContext.Groups.SingleOrDefaultAsync(g => g.Name == group.Name);
        if (g != null)
        {
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        _accountContext.Groups.Add(model);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Success;

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 更新分组信息，注意，如果修改分组名称，则同时需要更新用户表里面的分组名称
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<string> UpdateGroup([FromBody] HzgGroup group)
    {
        var response = new ResponseData()
        {
            Code = ErrorCode.Group_Not_Exist
        };

        var model = await _accountContext.Groups.SingleOrDefaultAsync(u => u.Id == group.Id);
        if (model == null)
        {
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        model.Name = group.Name;

        _accountContext.Groups.Update(model);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Success;

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    public async Task<string> delete(Guid? id)
    {
        var response = new ResponseData()
        {
            Code = ErrorCode.Group_Not_Exist
        };

        var group = await _accountContext.Groups.SingleOrDefaultAsync(u => u.Id == id);
        if (group == null)
        {
            // 用户不存在
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        // 需要检测是否存在用户关联
        var users = await _accountContext.UserGroups.AsNoTracking().Where(m => m.GroupId == group.Id).ToListAsync();
        if (users.Count > 0)
        {
            // 存在用户引用,无法删除
            response.Code = ErrorCode.Group_Has_User;

            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        // 删除关联的角色
        var groupRoles = await _accountContext.RoleGroups.Where(m => m.GroupId == id).ToListAsync();
        
        _accountContext.RoleGroups.RemoveRange(groupRoles);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Success;

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }
}