using Microsoft.EntityFrameworkCore;
using Hzg.Dto;
using Hzg.Models;
using Hzg.Data;

namespace Hzg.Tool;

public class MenuTool
{
    public static async Task<List<Menu>> GetUserPermissionMenus(AccountDbContext context, string userName)
    {
        // 获取菜单权限数据
        // 需要所有用户数据和菜单权限数据做对比，放到前端做对比
        // 这里只获取菜单权限数据
        var menuPermissions = await context.MenuPermissions.AsNoTracking().Where(m => m.UserName == userName).ToListAsync();

        // 根据权限数据获取 Menu 列表
        var menusToReturn = new List<Menu>();
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
    /// 生成前端目录树数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="menu"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static List<VueRouter> GenerateVueRouterData(List<Menu> data, Menu menu, Guid? id)
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
    /// <param name="data"></param>
    /// <param name="menu"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static List<MenuTreeNode> GenerateTreeData(List<Menu> data, Menu menu, Guid? id)
    {
        var resultJson = new List<MenuTreeNode>();

        var childrenData = data.Where(m => m.ParentMenuId == id).ToList();
        if (menu == null || id == null)
        {
            // 根节点
            childrenData = data.Where(m => m.IsRoot == true).ToList();
            if (childrenData.Count == 0 || childrenData == null)
            {
                return resultJson;
            }

            var rootList = new List<MenuTreeNode>();
            foreach(var item in childrenData)
            {
                rootList.AddRange(GenerateTreeData(data, item, item.Id));
            }

            return rootList;
        }

        // 非根节点
        var rootJson = new MenuTreeNode();
        rootJson.Id = menu.Id;
        rootJson.ParentMenuId = menu.ParentMenuId;
        rootJson.Label = menu.Title;
        rootJson.Url = menu.Url;
        rootJson.Name = menu.Name;
        rootJson.Path = menu.Path;
        rootJson.ComponentPath = menu.ComponentPath;

        var childrenList = new List<MenuTreeNode>();
        foreach(var item in childrenData)
        {
            childrenList.AddRange(GenerateTreeData(data, item, item.Id));
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