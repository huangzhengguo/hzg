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
/// 分组
/// </summary>
[ApiController]
[Authorize]
[Route("api/account/group/")]
// [ApiExplorerSettings(IgnoreApi = true)]
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
    /// 获取分组列表
    /// </summary>
    /// <param name="name">分组名称</param>
    /// <returns></returns>
    [HttpGet]
    [Route("get")]
    public async Task<ResponseData<List<HzgGroupInfoUpdateVo>>> List([FromQuery] string name)
    {
        var responseData = ResponseTool.FailedResponseData<List<HzgGroupInfoUpdateVo>>(showMsg: false);
        var groups = await _accountContext.Groups.AsNoTracking().Where(g => g.Name == name).OrderBy(g => g.Name).ToListAsync();
        if (string.IsNullOrWhiteSpace(name))
        {
            groups = await _accountContext.Groups.AsNoTracking().OrderBy(g => g.Name).ToListAsync();
        }

        responseData.Code = ErrorCode.Success;
        responseData.Data = groups.Select(g => new HzgGroupInfoUpdateVo
        {
            Id = g.Id,
            Name = g.Name,
            ParentId = g.ParentId,
        }).ToList();

        return responseData;
    }

    /// <summary>
    /// 获取分组目录树
    /// </summary>
    /// <param name="name">分组名称</param>
    /// <returns></returns>
    [HttpGet]
    [Route("list_tree")]
    public async Task<ResponseData<List<GroupTreeNode>>> ListTree([FromQuery] string name)
    {
        var responseData = ResponseTool.FailedResponseData<List<GroupTreeNode>>(showMsg: false);
        var groups = await _accountContext.Groups.AsNoTracking().Where(g => g.Name == name).OrderBy(g => g.Name).ToListAsync();
        if (string.IsNullOrWhiteSpace(name))
        {
            groups = await _accountContext.Groups.AsNoTracking().OrderBy(g => g.Name).ToListAsync();
        }

        var data = GroupTool.GenerateTreeData(groups, null);

        responseData.Code = ErrorCode.Success;
        responseData.Data = data;

        return responseData;
    }

    /// <summary>
    /// 新建分组
    /// </summary>
    /// <param name="group">分组信息</param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<ResponseData<HzgGroup>> Create([FromBody] HzgGroupInfoDto group)
    {
        var responseData = ResponseTool.FailedResponseData<HzgGroup>(showMsg: true);

        responseData.Code = ErrorCode.Group_Has_Exist;

        // 同一分组内分组名称不能相同
        var g = await _accountContext.Groups.AsNoTracking().SingleOrDefaultAsync(g => g.Name == group.Name && g.ParentId == group.ParentId);
        if (g != null)
        {
            return responseData;
        }

        var entity = new HzgGroup();

        entity.Id = Guid.NewGuid();
        entity.Name = group.Name;
        entity.ParentId = group.ParentId;
        entity.CreateTime = DateTime.Now;
        entity.CreatorId = await _userService.GetLoginUserId() ?? Guid.Empty;

        _accountContext.Groups.Add(entity);

        await _accountContext.SaveChangesAsync();

        responseData.Code = ErrorCode.Success;

        return responseData;
    }

    /// <summary>
    /// 更新分组信息
    /// </summary>
    /// <param name="group">分组信息</param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<ResponseData<HzgGroup>> Update([FromBody] HzgGroupInfoUpdateDto group)
    {
        var responseData = ResponseTool.FailedResponseData<HzgGroup>(showMsg: true);

        responseData.Code = ErrorCode.Group_Not_Exist;

        var model = await _accountContext.Groups.SingleOrDefaultAsync(u => u.Id == group.Id);
        if (model == null)
        {
            responseData.Message = "同一分组下存在相同名称分组";

            return responseData;
        }

        // 自己不能是自己的父分组
        if (model.Id == group.ParentId)
        {
            group.ParentId = model.ParentId;
        }

        // 获取所有子菜单列表
        var children = await GetChildrenGroups(group.Id);
        if (children.Where(m => m.Id == group.ParentId).Count() > 0)
        {
            // 如果父菜单是子菜单，则不做修改
            group.ParentId = model.ParentId;
        }

        model.ParentId = group.ParentId;
        model.Name = group.Name;
        model.UpdateTime = DateTime.Now;
        model.UpdateUser = await _userService.GetLoginUserId() ?? Guid.Empty;

        _accountContext.Groups.Update(model);

        await _accountContext.SaveChangesAsync();

        responseData.Code = ErrorCode.Success;
        responseData.Data = model;

        return responseData;
    }

    /// <summary>
    /// 删除分组
    /// </summary>
    /// <param name="id">分组标识</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    public async Task<ResponseData<bool>> Delete([FromQuery] Guid? id)
    {
        var responseData = ResponseTool.FailedResponseData<bool>(showMsg: true);

        responseData.Code = ErrorCode.Group_Not_Exist;
        responseData.Data = false;

        var group = await _accountContext.Groups.SingleOrDefaultAsync(u => u.Id == id);
        if (group == null)
        {
            // 分组不存在
            responseData.Message = "分组不存在";

            return responseData;
        }

        // 需要检测是否存在用户关联
        var users = await _accountContext.UserGroups.AsNoTracking().Where(m => m.GroupId == group.Id).ToListAsync();
        if (users.Count > 0)
        {
            // 存在用户引用,无法删除
            responseData.Code = ErrorCode.Group_Has_User;

            return responseData;
        }

        // 删除关联的角色
        var groupRoles = await _accountContext.RoleGroups.Where(m => m.GroupId == id).ToListAsync();
        
        _accountContext.RoleGroups.RemoveRange(groupRoles);
        _accountContext.Groups.Remove(group);

        await _accountContext.SaveChangesAsync();

        responseData.Code = ErrorCode.Success;
        responseData.Data = true;

        return responseData;
    }

    /// <summary>
    /// 获取指定分组的所有子分组
    /// </summary>
    /// <param name="id">分组标识</param>
    /// <returns></returns>
    async Task<List<HzgGroup>> GetChildrenGroups(Guid? id)
    {
        var result = new List <HzgGroup>();
        var childrenData = await _accountContext.Groups.AsNoTracking().Where(m => m.ParentId == id).ToListAsync();

        result.AddRange(childrenData);

        foreach (var child in childrenData)
        {
            var data = await _accountContext.Groups.AsNoTracking().Where(m => m.ParentId == child.Id).ToListAsync();

            result.AddRange(data);
        }

        return result;
    }
}