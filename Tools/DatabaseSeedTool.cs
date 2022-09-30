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
            adminMenu.ComponentPath = "adminmanagement/user/index";
            adminMenu.Name = "adminManagement";
            adminMenu.Path = "/adminManagement/";

            context.Menus.Add(adminMenu);
        }
        else
        {
            adminMenuId = menu.Id;
        }

        // 菜单管理
        var menuManagement = new Menu();

        menuManagement.Id = Guid.NewGuid();
        menuManagement.ParentMenuId = adminMenuId;
        menuManagement.Title = "菜单管理";
        menuManagement.IsRoot = false;
        menuManagement.IsFinal = true;
        menuManagement.Url = "#";
        menuManagement.ComponentPath = "adminmanagement/menu/index";
        menuManagement.Name = "MenuManagement";
        menuManagement.Path = "menuManagement";

        // 分组管理
        var groupManagement = new Menu();

        groupManagement.Id = Guid.NewGuid();
        groupManagement.ParentMenuId = adminMenuId;
        groupManagement.Title = "分组管理";
        groupManagement.IsRoot = false;
        groupManagement.IsFinal = true;
        groupManagement.Url = "#";
        groupManagement.ComponentPath = "adminmanagement/group/index";
        groupManagement.Name = "GroupManagement";
        groupManagement.Path = "groupManagement";

        // 分组管理
        var roleManagement = new Menu();

        roleManagement.Id = Guid.NewGuid();
        roleManagement.ParentMenuId = adminMenuId;
        roleManagement.Title = "角色管理";
        roleManagement.IsRoot = false;
        roleManagement.IsFinal = true;
        roleManagement.Url = "#";
        roleManagement.ComponentPath = "adminmanagement/role/index";
        roleManagement.Name = "RoleManagement";
        roleManagement.Path = "roleManagement";

        #region 产品管理
        var productManagement = new Menu();

        productManagement.Id = Guid.NewGuid();
        productManagement.Title = "产品管理";
        productManagement.IsRoot = true;
        productManagement.IsFinal = false;
        productManagement.Url = "#";
        productManagement.ComponentPath = "";
        productManagement.Name = "ProductManagement";
        productManagement.Path = "/productmanagement/";

        // 产品分类管理
        var productClassifyManagement = new Menu();

        productClassifyManagement.Id = Guid.NewGuid();
        productClassifyManagement.ParentMenuId = productManagement.Id;
        productClassifyManagement.Title = "产品分类";
        productClassifyManagement.IsRoot = false;
        productClassifyManagement.IsFinal = true;
        productClassifyManagement.Url = "#";
        productClassifyManagement.ComponentPath = "productmanagement/amazonproduct/productclassify";
        productClassifyManagement.Name = "ProductClassifyManagement";
        productClassifyManagement.Path = "productclassifymanagement";

        // 产品管理
        var productManagement1 = new Menu();

        productManagement1.Id = Guid.NewGuid();
        productManagement1.ParentMenuId = productManagement.Id;
        productManagement1.Title = "产品";
        productManagement1.IsRoot = false;
        productManagement1.IsFinal = true;
        productManagement1.Url = "#";
        productManagement1.ComponentPath = "productmanagement/amazonproduct/product";
        productManagement1.Name = "ProductClassifyProductManagement";
        productManagement1.Path = "productclassifyproductmanagement";

        #endregion
        Menu[] allMenus = { menuManagement, groupManagement, roleManagement, productManagement, productClassifyManagement, productManagement1 };
        Menu menuAdmin = null;
        foreach(var am in allMenus)
        {
            menuAdmin = await context.Menus.SingleOrDefaultAsync(m => m.ParentMenuId == am.ParentMenuId && m.Title == am.Title);
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