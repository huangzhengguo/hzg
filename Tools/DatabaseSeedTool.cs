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
        if (user == null)
        {
            var admin = new User();

            admin.Name = "Admin";
            admin.Nickname = "Nickname";
            admin.Salt = "Admin";
            admin.Password = MD5Tool.Encrypt("Admin", "Admin");
            admin.Email = "";
            admin.CreateTime = DateTime.Now;
            admin.CreateUser = "";

            context.Users.Add(admin);
        }

        // 后台管理
        var menu = await context.Menus.SingleOrDefaultAsync(m => m.Name == "后台管理");
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
        var menuAdmin = await context.Menus.SingleOrDefaultAsync(m => m.ParentMenuId == adminMenuId && m.Name == "菜单管理");
        var subMenuAdminId = Guid.NewGuid();
        if (menuAdmin == null)
        {
            var adminMenu = new Menu();

            adminMenu.Id = subMenuAdminId;
            adminMenu.ParentMenuId = adminMenuId;
            adminMenu.Title = "菜单管理";
            adminMenu.IsRoot = false;
            adminMenu.IsFinal = true;
            adminMenu.Url = "#";
            adminMenu.ComponentPath = "adminmanager/menu/index";
            adminMenu.Name = "MenuManager";
            adminMenu.Path = "menumanager";

            context.Menus.Add(adminMenu);
        }
        else
        {
            subMenuAdminId = menuAdmin.Id;
        }

        // 管理员添加后台管理权限
        var permission = await context.MenuPermissions.SingleOrDefaultAsync(mp => mp.RootMenuId == adminMenuId 
                                                                            && mp.SubMenuId == subMenuAdminId
                                                                            && mp.UserName == "admin");
        if (permission == null)
        {
            var adminMenuPersmission = new MenuPermission();

            // adminMenuPersmission.Id = Guid.NewGuid();
            adminMenuPersmission.UserName = "admin";
            adminMenuPersmission.RootMenuId = adminMenuId;
            adminMenuPersmission.SubMenuId = subMenuAdminId;
            adminMenuPersmission.Usable = true;

            context.MenuPermissions.Add(adminMenuPersmission);
        }

        await context.SaveChangesAsync();
    }
}