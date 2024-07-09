using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Hzg.Services;
using Hzg.Data;
using Hzg.Models;
using Hzg.Tool;
using Hzg.Consts;

namespace Hzg.Controllers;

/// <summary>
/// 后台菜单
/// </summary>
[Authorize]
[ApiController]
[Route("api/account/menu/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HzgMenuController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private readonly IUserService _userService;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="accountContext">用户数据上下文</param>
    /// <param name="userService">用户服务</param>
    public HzgMenuController(AccountDbContext accountContext, IUserService userService)
    {
        this._accountContext = accountContext;
        this._userService = userService;
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<string> Create([FromBody] HzgMenu menu)
    {
        if (menu == null)
        {
        }
        var result = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = null,
            ShowMsg = true
        };

        // 检测菜单是否已经存在
        // 1. 同一级的菜单标题不能相同
        // 2. 路由名称不能相同
        var m = await _accountContext.Menus.AsNoTracking().Where(m => m.ParentMenuId == menu.ParentMenuId && m.Title == menu.Title).ToListAsync();
        if (m != null && m.Count > 0)
        {
            result.Code = ErrorCode.Menu_Has_Exist;
            result.Message = "同一级的菜单标题不能相同";

            return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
        }

        m = await _accountContext.Menus.AsNoTracking().Where(m => m.Name == menu.Name).ToListAsync();
        if (m != null && m.Count > 0)
        {
            result.Code = ErrorCode.Menu_Has_Exist;
            result.Message = "已存在相同路由名称的菜单: " + m[0].Title;

            return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
        }
        
        // 根据是否有父菜单，判断是否是根菜单
        if (menu.ParentMenuId == null)
        {
            menu.IsRoot = true;
        }
        else
        {
            menu.IsRoot = false;
        }

        // 根据是否指定了组件路径，判断是否是叶子菜单
        if (string.IsNullOrEmpty(menu.ComponentPath) == false)
        {
            menu.IsFinal = true;
        }
        else
        {
            menu.IsFinal = false;
        }

        menu.CreatorId = await _userService.GetLoginUserId() ?? Guid.Empty;
        menu.CreateTime = DateTime.Now;

        _accountContext.Menus.Add(menu);
        var n = await _accountContext.SaveChangesAsync();
        if (n != 1)
        {
            result.Code = ErrorCode.Create_Failed;
        }

        return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<string> Update([FromBody] HzgMenu menu)
    {
        if (menu == null)
        {
        }
        var result = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = null
        };

        // 检测菜单是否已经存在
        var entity = await _accountContext.Menus.AsNoTracking().SingleOrDefaultAsync(m => m.Id == menu.Id);
        if (entity != null)
        {
            HZG_CommonTool.CopyProperties(menu, entity);

            if (entity.ParentMenuId == null)
            {
                entity.IsRoot = true;
                entity.ComponentPath = null;
            }
            else
            {
                entity.IsRoot = false;
            }

            entity.UpdateUser = await _userService.GetLoginUserId() ?? Guid.Empty;
            entity.UpdateTime = DateTime.Now;

            _accountContext.Update(entity);

            await _accountContext.SaveChangesAsync();

            // 是否更新菜单对应的权限
            return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
        }

        result.Code = ErrorCode.Failed;

        return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    public async Task<string>Delete(Guid id)
    {
        var result = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = null
        };

        var m = await _accountContext.Menus.SingleOrDefaultAsync(m => m.Id == id);
        if (m != null)
        {
            _accountContext.Menus.Remove(m);
        }

        var n = await _accountContext.SaveChangesAsync();
        if (n != 1)
        {
            // 删除失败
            result.Code = ErrorCode.Delete_Failed;
        }

        // 需要删除菜单关联的权限数据
        var mps = await _accountContext.MenuPermissions.Where(p => p.SubMenuId == id).ToListAsync();
        foreach(var mp in mps)
        {
            _accountContext.MenuPermissions.Remove(mp);
        }

        await _accountContext.SaveChangesAsync();

        return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get")]
    public async Task<string> Get()
    {
        var data = await _accountContext.Menus.AsNoTracking().ToListAsync();

        var jsonData = MenuTool.GenerateTreeData(data, null, null);
        var result = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = jsonData,
        };

        return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 获取用户菜单
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns></returns>
    [HttpGet]
    [Route("user-menu-permission")]
    public async Task<string>GetUserMenuPermission(string userName)
    {
        // 需要所有用户数据和菜单权限数据做对比，放到前端做对比
        // 这里只获取菜单权限数据
        var menuPermissions = await _accountContext.MenuPermissions.AsNoTracking().Where(m => m.UserName == userName).ToListAsync();

        // 根据权限数据获取 Menu 列表
        var menusToReturn = new List<HzgMenu>();
        var menus = await _accountContext.Menus.AsNoTracking().ToListAsync();
        foreach(var p in menuPermissions)
        {
            foreach(var m in menus)
            {
                if (m.Id == p.SubMenuId && p.Usable == true)
                {
                    menusToReturn.Add(m);
                }
            }
        }

        var routers = MenuTool.GenerateVueRouterData(menusToReturn, null, null);

        var responseData = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = routers
        };

        return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
    }
}