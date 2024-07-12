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
using Hzg.Vo;

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
    /// 获取角色列表
    /// </summary>
    /// <param name="name">角色信息</param>
    /// <returns></returns>
    [HttpGet]
    [Route("get")]
    public async Task<ResponseData<List<HzgRoleInfoVo>>> Get(string name)
    {
        var responseData = ResponseTool.FailedResponseData<List<HzgRoleInfoVo>>(showMsg: false);
        var roles = await _accountContext.Roles.AsNoTracking().Where(m => m.Name == name).OrderBy(m => m.Name).ToListAsync();
        if (string.IsNullOrWhiteSpace(name)) {
            roles = await _accountContext.Roles.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        responseData.Code = ErrorCode.Success;
        responseData.Data = roles.Select(g => new HzgRoleInfoVo
        {
            Id = g.Id,
            Name = g.Name
        }).ToList();

        return responseData;
    }

    /// <summary>
    /// 新建角色
    /// </summary>
    /// <param name="role">角色信息</param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<ResponseData<HzgRoleInfoVo>> Create([FromBody] HzgRoleInfoDto role)
    {
        var responseData = ResponseTool.FailedResponseData<HzgRoleInfoVo>(showMsg: true);
        var model = new HzgRole();

        model.Name = role.Name;
        model.Id = Guid.NewGuid();

        var u = await _accountContext.Roles.SingleOrDefaultAsync(m => m.Id == model.Id || m.Name == role.Name);
        if (u != null)
        {
            return responseData;
        }

        model.CreateTime = DateTime.Now;
        model.CreatorId = await _userService.GetLoginUserId() ?? Guid.Empty;

        _accountContext.Roles.Add(model);

        await _accountContext.SaveChangesAsync();

        responseData.Code = ErrorCode.Success;
        responseData.Data = new HzgRoleInfoVo
        {
            Id = model.Id,
            Name = model.Name
        }; 

        return responseData;
    }

    /// <summary>
    /// 更新角色信息
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<ResponseData<HzgRole>> Update([FromBody] HzgRoleInfoUpdateDto role)
    {
        var responseData = ResponseTool.FailedResponseData<HzgRole>(showMsg: true);

        var model = await _accountContext.Roles.SingleOrDefaultAsync(u => u.Id == role.Id);
        if (model == null)
        {
            // 角色不存在
            responseData.Message = "角色不存在";
            return responseData;
        }

        var sameNameEntity = await _accountContext.Roles.SingleOrDefaultAsync(u => u.Id != role.Id && u.Name == role.Name);
        if (sameNameEntity != null)
        {
            // 相同名称角色已存在
            responseData.Code = ErrorCode.Group_Has_Exist;
            responseData.Message = "相同名称角色已存在";

            return responseData;
        }

        // 角色信息
        model.Name = role.Name;
        model.UpdateTime = DateTime.Now;
        model.UpdateUser = await _userService.GetLoginUserId() ?? Guid.Empty;

        _accountContext.Roles.Update(model);

        await _accountContext.SaveChangesAsync();

        responseData.Code = ErrorCode.Success;
        responseData.Data = model;

        return responseData;
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id">标识</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    public async Task<ResponseData<bool>> Delete(Guid? id)
    {
        var responseData = ResponseTool.FailedResponseData<bool>(showMsg: true);

        responseData.Data = false;

        var role = await _accountContext.Roles.SingleOrDefaultAsync(u => u.Id == id);
        if (role == null)
        {
            // 角色不存在
            return responseData;
        }

        _accountContext.Remove(role);

        await _accountContext.SaveChangesAsync();

        responseData.Code = ErrorCode.Success;
        responseData.Data = true;

        return responseData;
    }

    /// <summary>
    /// 获取指定分组的角色
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get_group_roles")]
    public async Task<ResponseData<List<HzgRole>>> GetGroupRoles([FromQuery] string[] groupIds)
    {
        var responseData = ResponseTool.FailedResponseData<List<HzgRole>>(showMsg: false);
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

        responseData.Code = ErrorCode.Success;
        responseData.Data = roles;

        return responseData;
    }
}    