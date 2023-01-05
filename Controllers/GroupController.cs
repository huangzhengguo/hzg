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
        if (string.IsNullOrWhiteSpace(name)) {
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

        response.Code = ErrorCode.Create_Success;

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

        response.Code = ErrorCode.Delete_Group_Success;

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 获取分组角色目录树
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [HttpGet]
    // [Route("get-group-roles")]
    // public async Task<string> getGroupRoles(Guid? id) {
    //         // 格式 [{ label: '', children: [ { label: '', children: [] } ] }]
    //         var userTreeData = new List<TreeNodeModel>();
    //         var groups = _accountContext.Groups.AsNoTracking().ToList();
    //         foreach(var m in groups)
    //         {
    //             var nodeModel = new TreeNodeModel();

    //             nodeModel.Id = m.Id;
    //             nodeModel.ParentMenuId = null;
    //             nodeModel.Label = m.Name;
    //             nodeModel.IsLeaf = false;
    //             nodeModel.Disabled = true;

    //             // 获取分组成员
    //             var childrenUserTreeData = new List<TreeNodeModel>();
    //             var groupsRoles = await _accountContext.Roles.AsNoTracking().Where(u => u.GroupId == m.Id).ToListAsync();
    //             foreach(var gu in groupsRoles)
    //             {
    //                 var userNodeModel = new TreeNodeModel();
                    
    //                 userNodeModel.Id = gu.Id;
    //                 userNodeModel.ParentMenuId = m.Id;
    //                 userNodeModel.Label = gu.Name;
    //                 userNodeModel.IsLeaf = true;
    //                 userNodeModel.Disabled = false;

    //                 childrenUserTreeData.Add(userNodeModel);
    //             }

    //             nodeModel.Children = childrenUserTreeData.ToArray();

    //             userTreeData.Add(nodeModel);
    //         }

    //         var responseData = new ResponseData()
    //         {
    //             Code = ErrorCode.Success,
    //             Message = "获取成功",
    //             Data = userTreeData
    //         };

    //         return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
    // }
}