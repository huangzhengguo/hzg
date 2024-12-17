using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Hzg.Data;
using Hzg.Consts;
using Hzg.Models;
using Hzg.Services;
using Hzg.Tool;
using Hzg.Dto;
using Hzg.Vo;

namespace Hzg.Controllers;

/// <summary>
/// 用户
/// </summary>
[ApiController]
[Authorize]
[Route("api/account/user/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HzgUserController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private readonly IUserService _userService;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="accountContext">用户数据上下文</param>
    /// <param name="logger">日志</param>
    /// <param name="userService">用户服务</param>
    public HzgUserController(AccountDbContext accountContext, ILogger<HzgMenuPermission> logger, IUserService userService)
    {
        this._accountContext = accountContext;
        this._userService = userService;
    }

    /// <summary>
    /// 获取所有用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get")]
    public async Task<string> Get(string name)
    {
        var users = await _accountContext.Users.AsNoTracking().Where(m => m.Group == name).OrderBy(m => m.Name).ToListAsync();
        if (string.IsNullOrWhiteSpace(name)) {
            users = await _accountContext.Users.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        var userGroups = await _accountContext.UserGroups.AsNoTracking().ToListAsync();
        var userRoles = await _accountContext.UserRoles.AsNoTracking().ToListAsync();

        List<UserInfoDto> data = new List<UserInfoDto>();
        foreach(var u in users)
        {
            var userInfo = new UserInfoDto();

            userInfo.Id = u.Id;
            userInfo.Name = u.Name;
            userInfo.Gender = u.Gender;
            userInfo.Email = u.Email;

            // 分组 Id
            var groupIds = new List<Guid>();
            foreach(var ug in userGroups)
            {
                if (ug.UserId == u.Id)
                {
                    groupIds.Add(ug.GroupId);
                }
            }

            userInfo.GroupIds = groupIds.ToArray();

            // 角色 Id
            var roleIds = new List<Guid>();
            foreach(var ur in userRoles)
            {
                if (ur.UserId == u.Id)
                {
                    roleIds.Add(ur.RoleId);
                }
            }

            userInfo.RoleIds = roleIds.ToArray();

            data.Add(userInfo);
        }

        var response = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = data
        };

        return JsonSerializerTool.SerializeDefault(response);
    }

    /// <summary>
    /// 新建用户
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<ResponseData<bool>> Create([FromBody] UserEditDto user)
    {
        var response = new ResponseData<bool>()
        {
            Code = ErrorCode.User_Has_Exist,
            Data = false
        };

        var u = await _accountContext.Users.SingleOrDefaultAsync(m => m.Name == user.Name);
        if (u != null)
        {
            return response;
        }

        var model = new HzgUser();

        model.Id = Guid.NewGuid();
        model.Brand = user.Brand;
        model.Name = user.Name;
        model.Salt = RandomTool.GenerateDigitalAlphabetCode(6);
        model.Password = MD5Tool.Encrypt(user.Password, model.Salt);

        // 角色
        if (string.IsNullOrWhiteSpace(user.RoleId) == false)
        {
            var role = await _accountContext.Roles.Where(r => r.Id.ToString() == user.RoleId).SingleOrDefaultAsync();
            if (role != null)
            {
                model.RoleId = role.Id;
                model.Role = role.Name;
            }
        }

        // 分组
        if (string.IsNullOrWhiteSpace(user.GroupId) == false)
        {
            var group = await _accountContext.Groups.Where(r => r.Id.ToString() == user.GroupId).SingleOrDefaultAsync();
            if (group != null)
            {
                model.GroupId = group.Id;
                model.Group = group.Name;
            }
        }

        _accountContext.Users.Add(model);

        // // 添加分组关联
        // foreach(var gId in user.GroupIds)
        // {
        //     var userGroup = new HzgUserGroup()
        //     {
        //         UserId = model.Id,
        //         GroupId = new Guid(gId)
        //     };

        //     _accountContext.UserGroups.Add(userGroup);
        // }

        // // 添加角色关联
        // foreach(var rId in user.RoleIds)
        // {
        //     var userRole = new HzgUserRole()
        //     {
        //         UserId = model.Id,
        //         RoleId = new Guid(rId)
        //     };

        //     _accountContext.UserRoles.Add(userRole);
        // }

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Success;
        response.Data = true;

        return response;
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<ResponseData<bool>> Update([FromBody] UserEditDto user)
    {
        var response = new ResponseData<bool>()
        {
            Code = ErrorCode.User_Not_Exist,
            ShowMsg = true,
            Data = false
        };

        var model = await _accountContext.Users.SingleOrDefaultAsync(u => u.Id.ToString() == user.Id);
        if (model == null)
        {
            // 用户不存在
            return response;
        }

        // 用户信息
        model.Name = user.Name;

        // 角色
        if (string.IsNullOrWhiteSpace(user.RoleId) == false)
        {
            var role = await _accountContext.Roles.Where(r => r.Id.ToString() == user.RoleId).SingleOrDefaultAsync();
            if (role != null)
            {
                model.RoleId = role.Id;
                model.Role = role.Name;
            }
        }

        // 分组
        if (string.IsNullOrWhiteSpace(user.GroupId) == false)
        {
            var group = await _accountContext.Groups.Where(r => r.Id.ToString() == user.GroupId).SingleOrDefaultAsync();
            if (group != null)
            {
                model.GroupId = group.Id;
                model.Group = group.Name;
            }
        }

        _accountContext.Users.Update(model);

        // 关联分组
        var userGroups = await _accountContext.UserGroups.Where(ug => ug.UserId == model.Id).ToArrayAsync();
        var userGroupIds = userGroups.Where(ug => ug.UserId == model.Id).Select(ug => ug.GroupId.ToString());
        // var ids = GetIdsToAddAndRemove(user.GroupIds, userGroupIds, id =>
        // {                
        //     return new HzgUserGroup
        //     {
        //         UserId = model.Id,
        //         GroupId = new Guid(id)
        //     };
        // });

        // _accountContext.UserGroups.AddRange(ids.toAdd);

        // var guToDelete = userGroups.Where(ug => ids.idsToRmove.Contains(ug.GroupId.ToString()));

        // _accountContext.UserGroups.RemoveRange(guToDelete);

        // 关联角色
        var userRoles = await _accountContext.UserRoles.AsNoTracking().Where(ug => ug.UserId == model.Id).ToArrayAsync();
        var userRoleIds = userRoles.Where(ug => ug.UserId == model.Id).Select(ug => ug.RoleId.ToString());
        // var roleIds = GetIdsToAddAndRemove(user.RoleIds, userRoleIds, id =>
        // {                
        //     return new HzgUserRole
        //     {
        //         UserId = model.Id,
        //         RoleId = new Guid(id)
        //     };
        // });

        // _accountContext.UserRoles.AddRange(roleIds.toAdd);

        // var urToDelete = userRoles.Where(ur => roleIds.idsToRmove.Contains(ur.RoleId.ToString()));

        // _accountContext.UserRoles.RemoveRange(urToDelete);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Success;
        response.Data = true;

        return response;
    }

    /// <summary>
    /// 更新用户密码
    /// </summary>
    /// <param name="user">密码</param>
    /// <returns></returns>
    [HttpPut]
    [Route("reset_password")]
    public async Task<ResponseData<bool>> ResetPassword([FromBody] ResetUserPasswordDto user)
    {
        var response = new ResponseData<bool>()
        {
            Code = ErrorCode.User_Has_Exist,
            Data = false,
            ShowMsg = true
        };

        var entity = await _accountContext.Users.SingleOrDefaultAsync(u => u.Brand == user.Brand && u.Id.ToString() == user.Id);
        if (entity == null)
        {
            return response;
        }

        entity.Salt = RandomTool.GenerateDigitalAlphabetCode(6);
        entity.Password = MD5Tool.Encrypt(user.Password, entity.Salt);

        _accountContext.Update(entity);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Success;
        response.Data = true;

        return response;
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id">用户标识</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    public async Task<ResponseData<bool>> Delete([FromQuery] string id)
    {
        var response = new ResponseData<bool>()
        {
            Code = ErrorCode.User_Not_Exist,
            Data = false,
            ShowMsg = true
        };

        var user = await _accountContext.Users.SingleOrDefaultAsync(u => u.Id.ToString() == id);
        if (user == null)
        {
            // 用户不存在
            return response;
        }

        _accountContext.Remove(user);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.Success;
        response.Data = true;

        return response;
    }

    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="brand">品牌</param>
    /// <param name="page">当前页</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="keywords">查询关键词</param>
    /// <returns></returns>
    [HttpGet]
    [Route("list")] 
    public async Task<ResponseData<IEnumerable<HzgUserVo>>> List([FromQuery] string brand, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string keywords)
    {
        return await _userService.List(brand, page, pageSize, keywords);
    }

    /// <summary>
    /// 获取用户权限数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("user-menu-permission")]
    public async Task<string> GetUserMenuPermission()
    {
        // 需要所有用户数据和菜单权限数据做对比，放到前端做对比
        // 这里只获取菜单权限数据
        var id = await _userService.GetLoginUserId();
        var user = await _accountContext.Users.SingleOrDefaultAsync(u => u.Id == id);

        var menus = await MenuTool.GetRolePermissionMenus(_accountContext, user.Role);
        var responseData = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = menus
        };

        return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
    }

    private (IEnumerable<T> toAdd, IEnumerable<string> idsToRmove) GetIdsToAddAndRemove<T>(IEnumerable<string> ids, IEnumerable<string> currentIds, Func<string, T> addFunc)
    {
        // 要添加的
        var toAdd = new List<T>();
        foreach(var id in ids)
        {
            if (currentIds.Contains(id) == false)
            {
                toAdd.Add(addFunc(id));
            }
        }

        // 要移除的
        var idsToRamove = currentIds.Where(ug => ids.Contains(ug) == false);

        return (toAdd, idsToRamove);
    }
}    