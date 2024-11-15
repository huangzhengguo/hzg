using Microsoft.EntityFrameworkCore;
using Hzg.Dto;
using Hzg.Models;
using Hzg.Data;

namespace Hzg.Tool;

/// <summary>
/// 菜单工具类
/// </summary>
public class MenuTool
{
    /// <summary>
    /// 获取用户菜单权限
    /// </summary>
    /// <param name="context">数据库上下文</param>
    /// <param name="userName">用户名</param>
    /// <returns></returns>
    public static async Task<List<HzgMenu>> GetUserPermissionMenus(AccountDbContext context, string userName)
    {
        // 获取菜单权限数据
        // 需要所有用户数据和菜单权限数据做对比，放到前端做对比
        // 这里只获取菜单权限数据
        var menuPermissions = await context.MenuPermissions.AsNoTracking().Where(m => m.UserName == userName).ToListAsync();

        // 根据权限数据获取 Menu 列表
        var menusToReturn = new List<HzgMenu>();
        var menus = await context.Menus.AsNoTracking().ToListAsync();
        foreach(var p in menuPermissions)
        {
            foreach(var m in menus)
            {
                if ((m.Id == p.RootMenuId || m.Id == p.SubMenuId) && p.Usable == true)
                {
                    if (menusToReturn.Contains(m) == false)
                    {
                        menusToReturn.Add(m);
                    }
                }
            }
        }

        return menusToReturn;
    }

    /// <summary>
    /// 根据用户标识获取权限数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static async Task<List<HzgMenu>> GetUserPermissionMenus(AccountDbContext context, Guid? id)
    {
        // 获取菜单权限数据
        // 需要所有用户数据和菜单权限数据做对比，放到前端做对比
        // 这里只获取菜单权限数据
        var menuPermissions = await context.MenuPermissions.AsNoTracking().Where(m => m.UserId == id).ToListAsync();

        // 根据权限数据获取 Menu 列表
        var menusToReturn = new List<HzgMenu>();
        var menus = await context.Menus.AsNoTracking().ToListAsync();
        foreach(var p in menuPermissions)
        {
            foreach(var m in menus)
            {
                if ((m.Id == p.RootMenuId || m.Id == p.SubMenuId) && p.Usable == true)
                {
                    if (menusToReturn.Contains(m) == false)
                    {
                        menusToReturn.Add(m);
                    }
                }
            }
        }

        return menusToReturn;
    }

    /// <summary>
    /// 根据用户角色获取菜单权限数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public static async Task<List<HzgMenu>> GetRolePermissionMenus(AccountDbContext context, string role)
    {
        // 获取菜单权限数据
        // 需要所有用户数据和菜单权限数据做对比，放到前端做对比
        // 这里只获取菜单权限数据
        var menuPermissions = await context.MenuPermissions.AsNoTracking().Where(m => string.IsNullOrEmpty(role) == false && m.UserRole == role).ToListAsync();

        // 根据权限数据获取 Menu 列表
        var menusToReturn = new List<HzgMenu>();
        var menus = await context.Menus.AsNoTracking().ToListAsync();
        foreach(var p in menuPermissions)
        {
            foreach(var m in menus)
            {
                if (p.MenuId == m.Id && menusToReturn.Contains(m) == false)
                {
                    menusToReturn.Add(m);
                }
            }
        }

        return menusToReturn;
    }

    /// <summary>
    /// 生成前端目录树数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="menu"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static List<VueRouter> GenerateVueRouterData(List<HzgMenu> data, HzgMenu menu, Guid? id)
    {
        var resultJson = new List<VueRouter>();

        var childrenData = data.Where(m => m.ParentMenuId == id).ToList();
        if (menu == null || id == null)
        {
            // 根节点
            childrenData = data.Where(m => m.IsRoot == true).ToList();
            if (childrenData.Count == 0 || childrenData == null)
            {
                return resultJson;
            }

            var rootList = new List<VueRouter>();
            foreach(var item in childrenData)
            {
                rootList.AddRange(GenerateVueRouterData(data, item, item.Id));
            }

            return rootList;
        }

        // 非根节点
        var rootJson = new VueRouter();

        rootJson.Name = menu.Title;
        rootJson.Path = menu.Path;
        rootJson.Meta =
        new {
            title = menu.Title
        };

        var childrenList = new List<VueRouter>();
        foreach(var item in childrenData)
        {
            childrenList.AddRange(GenerateVueRouterData(data, item, item.Id));
        }
        
        rootJson.Children = childrenList.ToArray();

        resultJson.Add(rootJson);

        return resultJson;
    }

    /// <summary>
    /// 生成前端目录树数据
    /// </summary>
    /// <param name="data">要处理的数据</param>
    /// <param name="menu">单条菜单数据</param>
    /// <returns></returns>
    public static List<MenuTreeNode<T, ParentKeyT>> GenerateTreeData<T, ParentKeyT>(List<MenuTreeNode<T, ParentKeyT>> data, MenuTreeNode<T, ParentKeyT> menu)
    {
        var resultJson = new List<MenuTreeNode<T, ParentKeyT>>();

        List<MenuTreeNode<T, ParentKeyT>> childrenData;
        if (menu == null || menu.Id == null)
        {
            // 根节点
            childrenData = data.Where(m => m.ParentMenuId == null).ToList();
            if (childrenData.Count == 0 || childrenData == null)
            {
                return resultJson;
            }

            var rootList = new List<MenuTreeNode<T, ParentKeyT>>();
            foreach(var item in childrenData)
            {
                rootList.AddRange(GenerateTreeData<T, ParentKeyT>(data, item));
            }

            return rootList;
        }

        childrenData = data.Where(m => m.ParentMenuId.Equals(menu.Id)).ToList();

        // 非根节点
        var rootJson = new MenuTreeNode<T, ParentKeyT>
        {
            Id = menu.Id,
            ParentMenuId = menu.ParentMenuId,
            Label = menu.Title,
            Url = menu.Url,
            Name = menu.Name,
            Path = menu.Path,
            ComponentPath = menu.ComponentPath,
            Meta = menu.Meta
        };

        var childrenList = new List<MenuTreeNode<T, ParentKeyT>>();
        foreach(var item in childrenData)
        {
            childrenList.AddRange(GenerateTreeData(data, item));
        }

        if (childrenList.Count == 0)
        {
            rootJson.IsLeaf = true;
        }
        
        rootJson.Children = childrenList.ToArray();

        resultJson.Add(rootJson);

        return resultJson;
    }

    /// <summary>
    /// 生成前端目录树数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="menu"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static List<MenuTreeNode<Guid, Guid?>> GenerateMenuTree(List<HzgMenu> data, HzgMenu menu, Guid? id)
    {
        var resultJson = new List<MenuTreeNode<Guid, Guid?>>();

        var childrenData = data.Where(m => m.ParentMenuId == id).ToList();
        if (menu == null || id == null)
        {
            // 根节点
            childrenData = data.Where(m => m.ParentMenuId == null).ToList();
            if (childrenData.Count == 0 || childrenData == null)
            {
                return resultJson;
            }

            var rootList = new List<MenuTreeNode<Guid, Guid?>>();
            foreach(var item in childrenData)
            {
                rootList.AddRange(GenerateMenuTree(data, item, item.Id));
            }

            return rootList;
        }

        // 非根节点
        var rootJson = new MenuTreeNode<Guid, Guid?>
        {
            Id = menu.Id,
            ParentMenuId = menu.ParentMenuId,
            Label = menu.Title,
            Url = menu.Url,
            Name = menu.Name,
            Path = menu.Path,
            ComponentPath = menu.ComponentPath,
            Meta = menu.Meta
        };

        var childrenList = new List<MenuTreeNode<Guid, Guid?>>();
        foreach(var item in childrenData)
        {
            childrenList.AddRange(GenerateMenuTree(data, item, item.Id));
        }

        if (childrenList.Count == 0)
        {
            rootJson.IsLeaf = true;
        }
        
        rootJson.Children = childrenList.ToArray();

        resultJson.Add(rootJson);

        return resultJson;
    }
}