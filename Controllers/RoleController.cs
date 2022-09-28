using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Hzg.Data;
using Hzg.Const;
using Hzg.Models;
using Hzg.Services;
using Hzg.Tool;

namespace Hzg.Controllers;

/// <summary>
/// 角色
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]/")]
public class RoleController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private readonly IUserService _userService;
    public RoleController(AccountDbContext accountContext, IUserService userService)
    {
        this._accountContext = accountContext;
        this._userService = userService;
    }

    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get")]
    public async Task<string> get(string name)
    {
        var roles = await _accountContext.Roles.AsNoTracking().Where(m => m.Name == name).OrderBy(m => m.Name).ToListAsync();
        if (string.IsNullOrWhiteSpace(name)) {
            roles = await _accountContext.Roles.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        var response = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_Success,
            Message = "获取角色列表成功!",
            Data = roles
        };

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 新建角色
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<string> Create([FromBody] Role role)
    {
        var model = new Role();

        model.Name = role.Name;

        var response = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_HasExisted,
            Message = "角色已存在!"
        };

        var u = await _accountContext.Roles.SingleOrDefaultAsync(m => m.Id == model.Id);
        if (u != null)
        {
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        _accountContext.Roles.Add(model);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.ErrorCode_Success;
        response.Message = "创建成功!";

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 更新角色信息
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<string> update([FromBody] Role role)
    {
        var response = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_NotExist,
            Message = "分组不存在!"
        };

        var model = await _accountContext.Roles.SingleOrDefaultAsync(u => u.Id == role.Id);
        if (model == null)
        {
            // 角色不存在
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        // 角色信息
        model.Name = role.Name;

        _accountContext.Roles.Update(model);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.ErrorCode_Success;
        response.Message = "修改成功!";

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
            Code = ErrorCode.ErrorCode_NotExist,
            Message = "角色不存在!"
        };

        var role = await _accountContext.Roles.SingleOrDefaultAsync(u => u.Id == id);
        if (role == null)
        {
            // 角色不存在
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        _accountContext.Remove(role);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.ErrorCode_Success;
        response.Message = "删除成功!";

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 获取指定分组的
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get-group-roles")]
    public async Task<string> getGroupRoles(Guid groupId)
    {
        var roles = await _accountContext.Roles.AsNoTracking().Where(m => m.Id == groupId).OrderBy(m => m.Name).ToListAsync();

        var response = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_Success,
            Message = "获取角色列表成功!",
            Data = roles
        };

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }
}    