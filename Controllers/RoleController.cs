using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Hzg.Data;
using Hzg.Consts;
using Hzg.Models;
using Hzg.Services;
using Hzg.Tool;
using Hzg.Dto;

namespace Hzg.Controllers;

/// <summary>
/// 角色
/// </summary>
[ApiController]
[Authorize]
[Route("api/account/role/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HzgRoleController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private readonly IUserService _userService;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="accountContext">用户数据上下文</param>
    /// <param name="userService">用户服务</param>
    public HzgRoleController(AccountDbContext accountContext, IUserService userService)
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
            Code = ErrorCode.Success,
            Data = roles
        };

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 新建角色
    /// </summary>
    /// <param name="role">角色信息</param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<string> Create([FromBody] HzgRole role)
    {
        var model = new HzgRole();

        model.Name = role.Name;

        var response = new ResponseData()
        {
            Code = ErrorCode.Role_Has_Exist
        };

        var u = await _accountContext.Roles.SingleOrDefaultAsync(m => m.Id == model.Id);
        if (u != null)
        {
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        _accountContext.Roles.Add(model);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Create_Success;

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 更新角色信息
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<string> update([FromBody] HzgRole role)
    {
        var response = new ResponseData()
        {
            Code = ErrorCode.Group_Not_Exist
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

        response.Code = ErrorCode.Update_Success;

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
            Code = ErrorCode.Role_Not_Exist
        };

        var role = await _accountContext.Roles.SingleOrDefaultAsync(u => u.Id == id);
        if (role == null)
        {
            // 角色不存在
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        _accountContext.Remove(role);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Delete_Success;

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 获取指定分组的角色
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get-group-roles")]
    public async Task<string> getGroupRoles([FromQuery] string[] groupIds)
    {
        // 分组角色关联信息
        var roleGroupsIds = await _accountContext.RoleGroups.AsNoTracking().Where(rg => groupIds.Contains(rg.GroupId.ToString())).Select(rg => rg.RoleId).ToListAsync();
        // 所有角色
        var roles = await _accountContext.Roles.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        var roleInfos = new List<RoleInfoDto>();
        foreach(var r in roles)
        {
            if (roleGroupsIds.Contains(r.Id) == true)
            {
                continue;
            }

            var dto = new RoleInfoDto();

            dto.Id = r.Id;
            dto.Name = r.Name;

            roleInfos.Add(dto);
        }

        var response = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = roles
        };

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }
}    