using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Hzg.Data;
using Hzg.Models;
using Hzg.Consts;
using Hzg.Tool;
using Hzg.Services;
using Hzg.Dto;

namespace Admin.Controllers;

/// <summary>
/// 菜单权限
/// </summary>
[Authorize]
[ApiController]
[Route("api/account/menu_permission/")]
// [ApiExplorerSettings(IgnoreApi = true)]
public class HzgMenuPermissionController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private readonly IUserService _userService;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="accountContext">用户数据上下文</param>
    /// <param name="logger">日志</param>
    /// <param name="userService">用户服务</param>
    public HzgMenuPermissionController(AccountDbContext accountContext, ILogger<HzgMenuPermission> logger, IUserService userService)
    {
        this._accountContext = accountContext;
        this._userService = userService;
    }

    /// <summary>
    /// 获取指定菜单的权限数据
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("get-menu-permissions")]
    public async Task<string> GetMenuPermissions(Guid? menuId)
    {
        var responseData = new ResponseData()
        {
            Code = ErrorCode.Success
        };

        var permissions = await _accountContext.MenuPermissions.AsNoTracking().Where(m => m.SubMenuId == menuId).ToListAsync();

        responseData.Data = permissions;

        return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 获取指定角色的菜单权限
    /// </summary>
    /// <param name="role">角色名称</param>
    /// <returns></returns>
    [HttpGet]
    [Route("get_role_permission")]
    public async Task<ResponseData<List<HzgMenuPermission>>> GetRoleMenuPermissionsAsync([FromQuery] string role)
    {
        var responseData = ResponseTool.FailedResponseData<List<HzgMenuPermission>>(showMsg: false);

        var permissions = await _accountContext.MenuPermissions.AsNoTracking().Where(m => m.UserRole == role).ToListAsync();

        responseData.Code = ErrorCode.Success;
        responseData.Data = permissions;

        return responseData;
    }

    /// <summary>
    /// 新增权限
    /// </summary>
    /// <param name="menuPermission"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create-menu-permission")]
    public async Task<string> CreateMenuPermission([FromBody] HzgMenuPermission menuPermission)
    {
        var responseData = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = null
        };

        if (menuPermission != null && menuPermission.Usable == true)
        {
            var mp = await _accountContext.MenuPermissions.SingleOrDefaultAsync(m => m.SubMenuId == menuPermission.SubMenuId && m.UserId == menuPermission.UserId);
            if (mp == null)
            {
                var userId = await _userService.GetLoginUserId();

                menuPermission.CreatorId = userId.Value;
                menuPermission.CreateTime = DateTime.Now;

                _accountContext.MenuPermissions.Add(menuPermission);

                int resultRowCount = await _accountContext.SaveChangesAsync();
                if (resultRowCount == 1)
                {
                    return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
                }
            }
        }

        responseData.Code = ErrorCode.Failed;

        return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 更新分组权限，根据用户的分组和角色增加、删除菜单权限
    /// </summary>
    /// <param name="permissionsToUpdate">要添加的权限</param>
    /// <returns></returns>
    [HttpPost]
    [Route("update_permission")]
    public async Task<ResponseData<bool>> UpdateMenuPermission([FromBody] HzgMenuPermissionUpdateDto permissionsToUpdate)
    {
        var responseData = ResponseTool.FailedResponseData<bool>(showMsg: true);
        var userId = await _userService.GetLoginUserId();
        var permissionsToAdd = new List<HzgMenuPermission>();

        // 获取对应角色的菜单权限
        var permissions = await _accountContext.MenuPermissions.Where(m => m.UserRole == permissionsToUpdate.Role).ToListAsync();
        var menuIds = permissions.Select(m => m.MenuId.ToString()).ToList();
        foreach(var menuId in permissionsToUpdate.MenuIds)
        {
            if (menuIds.Contains(menuId) == false)
            {
                // 不存在则增加权限
                var m = new HzgMenuPermission();

                m.Id = Guid.NewGuid();
                m.MenuId = new Guid(menuId);
                m.CreateTime = DateTime.Now;
                m.CreatorId = userId ?? Guid.Empty;
                m.GroupId = null;
                m.RoleId = permissionsToUpdate.RoleId;
                m.UserRole = permissionsToUpdate.Role;

                permissionsToAdd.Add(m);
            }
        }

        _accountContext.MenuPermissions.AddRange(permissionsToAdd);

        foreach(var menuId in menuIds)
        {
            if (permissionsToUpdate.MenuIds.Contains(menuId) == false)
            {
                _accountContext.MenuPermissions.RemoveRange(permissions.Where(m => m.MenuId.ToString() == menuId));
            }
        }

        await _accountContext.SaveChangesAsync();

        responseData.Code = ErrorCode.Success;
        responseData.Data = true;

        return responseData;
    }

    /// <summary>
    /// 删除权限
    /// </summary>
    /// <param name="menuPermission"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete-menu-permission")]
    public async Task<string> DeleteMenuPermission([FromBody] HzgMenuPermission menuPermission)
    {
        var responseData = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = null
        };

        if (menuPermission != null && menuPermission.Usable == false)
        {
            var data = await _accountContext.MenuPermissions.Where(mp => mp.SubMenuId == menuPermission.SubMenuId && mp.UserGroup == menuPermission.UserGroup && mp.UserId == menuPermission.UserId).ToListAsync();
            if (data != null)
            {
                foreach(var m in data)
                {
                    _accountContext.Remove(m);
                }
            }

            await _accountContext.SaveChangesAsync();

            return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
        } 

        responseData.Code = ErrorCode.Failed;

        return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
    }
}