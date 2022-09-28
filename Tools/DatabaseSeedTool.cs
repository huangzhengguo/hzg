using Microsoft.EntityFrameworkCore;
using Hzg.Data;
using Hzg.Models;


namespace Hzg.Tool;

/// <summary>
/// 数据库
/// </summary>
public static class DatabaseSeedTool
{
    /// <summary>
    /// 添加管理员账号
    /// </summary>
    public static async void SeedAdminUser(AccountDbContext context)
    {
        // 管理员账号
        var user = await context.Users.SingleOrDefaultAsync(u => u.Name == "Admin");
        Guid userId;
        if (user == null)
        {
            userId = Guid.NewGuid();

            var admin = new User();

            admin.Id = userId;
            admin.Name = "Admin";
            admin.Nickname = "Nickname";
            admin.Salt = "Admin";
            admin.Password = MD5Tool.Encrypt("Admin", "Admin");
            admin.Email = "";
            admin.CreateTime = DateTime.Now;
            admin.CreateUser = "";

            context.Users.Add(admin);
        }
        else
        {
            userId = user.Id;
        }

        // 后台管理
        var menu = await context.Menus.SingleOrDefaultAsync(m => m.Title == "后台管理");
        var adminMenuId = Guid.NewGuid();
        if (menu == null)
        {
            var adminMenu = new Menu();

            adminMenu.Id = adminMenuId;
            adminMenu.Title = "后台管理";
            adminMenu.IsRoot = true;
            adminMenu.IsFinal = false;
            adminMenu.Url = "#";
            adminMenu.ComponentPath = "adminmanager/user/index";
            adminMenu.Name = "adminManager";
            adminMenu.Path = "/adminmanager/";

            context.Menus.Add(adminMenu);
        }
        else
        {
            adminMenuId = menu.Id;
        }

        // 菜单管理
        var menuManager = new Menu();

        menuManager.Id = Guid.NewGuid();
        menuManager.ParentMenuId = adminMenuId;
        menuManager.Title = "菜单管理";
        menuManager.IsRoot = false;
        menuManager.IsFinal = true;
        menuManager.Url = "#";
        menuManager.ComponentPath = "adminmanager/menu/index";
        menuManager.Name = "MenuManager";
        menuManager.Path = "menumanager";

        // 分组管理
        var groupManager = new Menu();

        groupManager.Id = Guid.NewGuid();
        groupManager.ParentMenuId = adminMenuId;
        groupManager.Title = "分组管理";
        groupManager.IsRoot = false;
        groupManager.IsFinal = true;
        groupManager.Url = "#";
        groupManager.ComponentPath = "adminmanager/group/index";
        groupManager.Name = "GroupManager";
        groupManager.Path = "groupmanager";

        // 分组管理
        var roleManager = new Menu();

        roleManager.Id = Guid.NewGuid();
        roleManager.ParentMenuId = adminMenuId;
        roleManager.Title = "角色管理";
        roleManager.IsRoot = false;
        roleManager.IsFinal = true;
        roleManager.Url = "#";
        roleManager.ComponentPath = "adminmanager/role/index";
        roleManager.Name = "RoleManager";
        roleManager.Path = "rolemanager";

        Menu[] allMenus = { menuManager, groupManager, roleManager };
        Menu menuAdmin = null;
        foreach(var am in allMenus)
        {
            menuAdmin = await context.Menus.SingleOrDefaultAsync(m => m.ParentMenuId == adminMenuId && m.Title == am.Title);
            Guid subMenuAdminId;
            if (menuAdmin == null)
            {
                subMenuAdminId = am.Id;
                context.Menus.Add(am);
            }
            else
            {
                subMenuAdminId = menuAdmin.Id;
            }

            // 添加权限
            var permission = await context.MenuPermissions.SingleOrDefaultAsync(mp => mp.RootMenuId == adminMenuId 
                                                                                && mp.SubMenuId == subMenuAdminId
                                                                                && mp.UserId == userId);
            if (permission == null)
            {
                var adminMenuPersmission = new MenuPermission();

                // adminMenuPersmission.Id = Guid.NewGuid();
                adminMenuPersmission.RootMenuId = adminMenuId;
                adminMenuPersmission.SubMenuId = subMenuAdminId;
                adminMenuPersmission.Usable = true;
                adminMenuPersmission.UserId = userId;
                adminMenuPersmission.CreateTime = DateTime.Now;

                context.MenuPermissions.Add(adminMenuPersmission);
            }
        }

        await context.SaveChangesAsync();
    }
}