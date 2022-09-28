using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Hzg.Data;
using Hzg.Const;
using Hzg.Models;
using Hzg.Services;
using Hzg.Tool;

namespace Hzg.Controllers;

/// <summary>
/// 用户
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]/")]
public class UserController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private readonly IUserService _userService;
    public UserController(AccountDbContext accountContext, ILogger<MenuPermission> logger, IUserService userService)
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
    public async Task<string> get(string name)
    {
        var users = await _accountContext.Users.AsNoTracking().Where(m => m.Name == name).OrderBy(m => m.Name).ToListAsync();
        if (string.IsNullOrWhiteSpace(name)) {
            users = await _accountContext.Users.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        var response = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_Success,
            Message = "获取用户列表成功!",
            Data = users
        };

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 新建用户
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<string> Create([FromBody] User user)
    {
        var model = new User();

        model.Name = user.Name;
        model.Gender = user.Gender;
        model.Salt = RandomTool.GenerateDigitalAlphabetCode(6);
        model.Password = MD5Tool.Encrypt(user.Password, model.Salt);

        var response = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_HasExisted,
            Message = "用户已存在!"
        };

        var u = await _accountContext.Users.SingleOrDefaultAsync(m => m.Name == user.Name);
        if (u != null)
        {
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        _accountContext.Users.Add(model);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.ErrorCode_Success;
        response.Message = "创建成功!";

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("update")]
    public async Task<string> update([FromBody] User user)
    {
        var response = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_NotExist,
            Message = "用户不存在!"
        };

        var model = await _accountContext.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
        if (model == null)
        {
            // 用户不存在
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        // 用户信息
        model.Name = user.Name;
        model.Gender = user.Gender;

        _accountContext.Users.Update(model);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.ErrorCode_Success;
        response.Message = "修改成功!";

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    public async Task<string> delete(Guid? id)
    {
        var response = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_NotExist,
            Message = "用户不存在!"
        };

        var user = await _accountContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            // 用户不存在
            return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
        }

        _accountContext.Remove(user);

        await _accountContext.SaveChangesAsync();

        response.Code = ErrorCode.ErrorCode_Success;
        response.Message = "删除成功!";

        return JsonSerializer.Serialize(response, JsonSerializerTool.DefaultOptions());
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
        var id = await _userService.GetCurrentUserId();

        var menus = await MenuTool.GetUserPermissionMenus(_accountContext, id);
        var responseData = new ResponseData()
        {
            Code = ErrorCode.ErrorCode_Success,
            Message = "获取成功",
            Data = menus
        };

        return JsonSerializer.Serialize(responseData, JsonSerializerTool.DefaultOptions());
    }
}    