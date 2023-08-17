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

            var admin = new HzgUser();

            admin.Id = userId;
            admin.Name = "Admin";
            admin.Nickname = "Nickname";
            admin.Salt = "Admin";
            admin.Brand = "RADIANS";
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
            var adminMenu = new HzgMenu();

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
        var menuManagement = new HzgMenu();

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
        var groupManagement = new HzgMenu();

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
        var roleManagement = new HzgMenu();

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
        var productManagement = await context.Menus.SingleOrDefaultAsync(m => m.Title == "产品管理");
        if (productManagement == null)
        {
            productManagement = new HzgMenu();
            productManagement.Id = Guid.NewGuid();
        }

        productManagement.Title = "产品管理";
        productManagement.IsRoot = true;
        productManagement.IsFinal = false;
        productManagement.Url = "#";
        productManagement.ComponentPath = "";
        productManagement.Name = "ProductManagement";
        productManagement.Path = "/productmanagement/";

        // 产品分类管理
        var productClassifyManagement = new HzgMenu();

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
        var productManagement1 = new HzgMenu();

        productManagement1.Id = Guid.NewGuid();
        productManagement1.ParentMenuId = productManagement.Id;
        productManagement1.Title = "产品";
        productManagement1.IsRoot = false;
        productManagement1.IsFinal = true;
        productManagement1.Url = "#";
        productManagement1.ComponentPath = "productmanagement/amazonproduct/product";
        productManagement1.Name = "ProductClassifyProductManagement";
        productManagement1.Path = "productclassifyproductmanagement";
        productManagement1.Meta = "{\"keepAlive\": false}";

        // 产品轮播图管理
        var productCarouselManagement = new HzgMenu();

        productCarouselManagement.Id = Guid.NewGuid();
        productCarouselManagement.ParentMenuId = productManagement.Id;
        productCarouselManagement.Title = "产品轮播图";
        productCarouselManagement.IsRoot = false;
        productCarouselManagement.IsFinal = true;
        productCarouselManagement.Url = "#";
        productCarouselManagement.ComponentPath = "productmanagement/amazonproduct/productcarousel";
        productCarouselManagement.Name = "ProductCarouselProductManagement";
        productCarouselManagement.Path = "productcarouselproductmanagement";
        productCarouselManagement.Meta = "{\"keepAlive\": false}";

        #endregion

        #region 物联网产品管理
        var iotProductManagement = await GenerateMenu(context, "物联网产品", true, false, "IotProductManagement", "", "/iotproductmanagement/");

        // 产品管理
        var iotMainProductManagement = await GenerateMenu(context, "物联网主产品", false, true, "IotMainProductManagement", "productmanagement/iotproduct/iotmainproduct", "/iotproduct/iotmainproduct/", true, iotProductManagement.Id);
        var iotSubProductManagement = await GenerateMenu(context, "物联网子产品", false, true, "IotSubProductManagement", "productmanagement/iotproduct/iotsubproduct", "/iotproduct/iotsubproduct/", true, iotProductManagement.Id);

        #endregion

        // 设备管理
        var deviceManagement = await GenerateMenu(context, "设备管理", true, false, "DeviceManagement", "", "/device/");
        var subDeviceManagement = await GenerateMenu(context, "设备", false, true, "SubDeviceManagement", "device/device", "device", true, deviceManagement.Id);

        // 账户管理
        var accountManagement = await GenerateMenu(context, "账号管理", true, false, "AccountManagement", "", "/account/");
        var userManagement = await GenerateMenu(context, "用户", false, true, "UserManagement", "account/user", "user", true, accountManagement.Id);
        var iotGroupManagement = await GenerateMenu(context, "分组", false, true, "AccountGroupManagement", "account/group", "group", true, accountManagement.Id);

        // 文件
        // 说明书
        var fileManagement = await GenerateMenu(context, "文件", true, false, "fileManagement", "", "/file/");
        var subInstructionManagement = await GenerateMenu(context, "产品说明书", false, true, "SubInstructionManagement", "instruction/instruction", "instruction", true, fileManagement.Id);

        // FAQ
        var subFaqnManagement = await GenerateMenu(context, "APP FAQ", false, true, "SubFaqManagement", "faq/faq", "faq", true, fileManagement.Id);

        // 固件
        var subFirmwareManagement = await GenerateMenu(context, "产品固件", false, true, "SubFirmwareManagement", "firmware/firmware", "firmware", true, fileManagement.Id);

        // 客户服务
        var serviceManagement = await GenerateMenu(context, "售后", true, false, "ServiceManagement", "", "/service/");
        var customerServiceManagement = await GenerateMenu(context, "客户反馈", false, true, "CustomerServiceManagement", "service/feedback", "feedback", true, serviceManagement.Id);

        HzgMenu[] allMenus = { menuManagement,
                            groupManagement, 
                            roleManagement, 
                            productManagement, 
                            productClassifyManagement, 
                            productManagement1, 
                            productCarouselManagement,
                            iotProductManagement,
                            iotMainProductManagement,
                            iotSubProductManagement,
                            deviceManagement,
                            subDeviceManagement,
                            accountManagement,
                            userManagement,
                            iotGroupManagement,
                            fileManagement,
                            subInstructionManagement,
                            subFaqnManagement,
                            subFirmwareManagement,
                            serviceManagement,
                            customerServiceManagement
                            };
        HzgMenu menuAdmin = null;
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
                var adminMenuPersmission = new HzgMenuPermission();

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

    /// <summary>
    /// 生成菜单项
    /// </summary>
    /// <param name="context">数据</param>
    /// <param name="title">标题</param>
    /// <param name="isRoot">是否是根</param>
    /// <param name="isFinal">是否是页</param>
    /// <param name="name">名称</param>
    /// <param name="componentPath">组件路径</param>
    /// <param name="path">路由路径</param>
    /// <param name="keepAlive">是否保活</param>
    /// <param name="parentMenuId">父菜单路径</param>
    /// <returns></returns>
    public static async Task<HzgMenu> GenerateMenu(AccountDbContext context, string title, bool isRoot, bool isFinal, string name, string componentPath, string path, bool keepAlive = false, Guid? parentMenuId = null)
    {
        var menu = await context.Menus.SingleOrDefaultAsync(m => m.Title == title);
        if (menu == null)
        {
            menu = new HzgMenu();
            menu.Id = Guid.NewGuid();

            if (isRoot == false)
            {
                menu.ParentMenuId = parentMenuId;
            }
        }

        menu.Title = title;
        menu.IsRoot = isRoot;
        menu.IsFinal = false;
        menu.Url = "#";
        menu.Name = name;
        menu.ComponentPath = componentPath;
        menu.Path = path;
        if (keepAlive == true)
        {
            menu.Meta = "{\"keepAlive\": false}";
        }

        return menu;
    }
}