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

namespace Ledinpro.Controllers;

/// <summary>
/// 菜单权限
/// </summary>
[Authorize]
[ApiController]
[Route("api/account/menu_permission/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class MenuPermissionController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private readonly IUserService _userService;
    public MenuPermissionController(AccountDbContext accountContext, ILogger<HzgMenuPermission> logger, IUserService userService)
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