using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Hzg.Services;
using Hzg.Data;
using Hzg.Models;
using Hzg.Tool;
using Hzg.Consts;
using Hzg.Dto;

namespace Hzg.Controllers;

/// <summary>
/// 后台菜单
/// </summary>
[Authorize]
[ApiController]
[Route("api/account/menu/")]
// [ApiExplorerSettings(IgnoreApi = true)]
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
    /// <param name="menu">菜单信息</param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<ResponseData<HzgMenu>> Create([FromBody] HzgMenuInfoDto menu)
    {
        var responseData = ResponseTool.FailedResponseData<HzgMenu>(showMsg: true);

        responseData.Code = ErrorCode.Success;

        // 检测菜单是否已经存在
        // 1. 同一级的菜单标题不能相同
        // 2. 路由名称不能相同
        var m = await _accountContext.Menus.AsNoTracking().Where(m => m.ParentMenuId == menu.ParentMenuId &&
                                                                 m.Title == menu.Title).ToListAsync();
        if (m != null && m.Count > 0)
        {
            responseData.Code = ErrorCode.Menu_Has_Exist;
            responseData.Message = "同一级的菜单标题不能相同";

            return responseData;
        }

        m = await _accountContext.Menus.AsNoTracking().Where(m => m.Name == menu.Name).ToListAsync();
        if (m != null && m.Count > 0)
        {
            responseData.Code = ErrorCode.Menu_Has_Exist;
            responseData.Message = "已存在相同路由名称的菜单: " + m[0].Title;

            return responseData;
        }

        var entity = new HzgMenu();

        HZG_CommonTool.CopyProperties(menu, entity);

        entity.Id = Guid.NewGuid();
        entity.CreatorId = await _userService.GetLoginUserId() ?? Guid.Empty;
        entity.CreateTime = DateTime.Now;

        if (entity.ParentMenuId == null)
        {
            entity.IsRoot = true;
            entity.ComponentPath = null;
        }
        else
        {
            entity.IsRoot = false;
        }

        _accountContext.Menus.Add(entity);
        var n = await _accountContext.SaveChangesAsync();
        if (n != 1)
        {
            responseData.Code = ErrorCode.Create_Failed;
        }

        responseData.Data = await _accountContext.Menus.AsNoTracking().SingleOrDefaultAsync(m => m.Id == entity.Id);

        return responseData;
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="menu">菜单信息</param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<ResponseData<HzgMenu>> Update([FromBody] HzgMenuInfoUpdateDto menu)
    {
        var responseData = ResponseTool.FailedResponseData<HzgMenu>(showMsg: true);

        responseData.Code = ErrorCode.Success;

        // 检测菜单是否已经存在
        var entity = await _accountContext.Menus.SingleOrDefaultAsync(m => m.Id == menu.Id);
        if (entity != null)
        {
            // 父菜单不能是自己及子菜单
            if (menu.ParentMenuId == menu.Id)
            {
                // 不做修改
                menu.ParentMenuId = entity.ParentMenuId;
            }

            // 获取所有子菜单列表
            var children = await GetChildrenMenus(menu.Id);
            if (children.Where(m => m.Id == menu.ParentMenuId).Count() > 0)
            {
                // 如果父菜单是子菜单，则不做修改
                menu.ParentMenuId = entity.ParentMenuId;
            }

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

            responseData.Data = await _accountContext.Menus.AsNoTracking().SingleOrDefaultAsync(m => m.Id == entity.Id);

            // 是否更新菜单对应的权限
            return responseData;
        }

        responseData.Code = ErrorCode.Failed;

        return responseData;
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id">菜单标识</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    public async Task<ResponseData<bool>>Delete(Guid id)
    {
        var responseData = ResponseTool.FailedResponseData<bool>(showMsg: true);

        responseData.Data = false;

        // 如果包含子菜单，则禁止删除
        var childrenMenus = await _accountContext.Menus.AsNoTracking().Where(m => m.ParentMenuId == id).ToListAsync();
        if (childrenMenus.Count > 0)
        {
            responseData.Code = ErrorCode.Delete_Failed;
            
            responseData.Message = "请先删除子菜单";

            return responseData;
        }

        var m = await _accountContext.Menus.SingleOrDefaultAsync(m => m.Id == id);
        if (m != null)
        {
            _accountContext.Menus.Remove(m);
        }

        var n = await _accountContext.SaveChangesAsync();
        if (n != 1)
        {
            // 删除失败
            responseData.Code = ErrorCode.Delete_Failed;
        }

        // 需要删除菜单关联的权限数据
        var mps = await _accountContext.MenuPermissions.Where(p => p.SubMenuId == id).ToListAsync();
        foreach(var mp in mps)
        {
            _accountContext.MenuPermissions.Remove(mp);
        }

        await _accountContext.SaveChangesAsync();

        responseData.Code = ErrorCode.Success;
        responseData.Data = true;

        return responseData;
    }

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get")]
    public async Task<ResponseData<List<MenuTreeNode<Guid, Guid?>>>> Get()
    {
        var responseData = ResponseTool.FailedResponseData<List<MenuTreeNode<Guid, Guid?>>>(showMsg: false);
        var data = await _accountContext.Menus.AsNoTracking().ToListAsync();

        var menuTreeNodeList = MenuTool.GenerateMenuTree(data, null, null);
       
        responseData.Code = ErrorCode.Success;
        responseData.Data = menuTreeNodeList;

        return responseData;
    }

    /// <summary>
    /// 获取用户菜单
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns></returns>
    [HttpGet]
    [Route("user-menu-permission")]
    public async Task<ResponseData<List<VueRouter>>>GetUserMenuPermission(string userName)
    {
        var responseData = ResponseTool.FailedResponseData<List<VueRouter>>(showMsg: false);
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

        responseData.Code = ErrorCode.Success;
        responseData.Data = routers;

        return responseData;
    }

    /// <summary>
    /// 获取指定菜单的所有子菜单
    /// </summary>
    /// <param name="id">菜单标识</param>
    /// <returns></returns>
    async Task<List<HzgMenu>> GetChildrenMenus(Guid? id)
    {
        var result = new List <HzgMenu>();
        var childrenData = await _accountContext.Menus.AsNoTracking().Where(m => m.ParentMenuId == id).ToListAsync();

        result.AddRange(childrenData);

        foreach (var child in childrenData)
        {
            var data = await _accountContext.Menus.AsNoTracking().Where(m => m.ParentMenuId == child.Id).ToListAsync();

            result.AddRange(data);
        }

        return result;
    }
}