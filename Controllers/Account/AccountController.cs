using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Hzg.Data;
using Hzg.Services;
using Hzg.Models;
using Hzg.Consts;
using Hzg.Tool;

namespace Inledco.Controllers;

/// <summary>
/// 账号
/// </summary>
[Authorize]
[ApiController]
[Route("api/account/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HzgAccountController : ControllerBase
{
    private readonly AccountDbContext _accountContext;
    private IConfiguration _configuration;
    private readonly IJwtService _jwtService;
    private readonly ILogger<LoginViewModel> _logger;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IVerifyCodeService _verifyCodeService;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="inledcoContext">数据库上下文</param>
    /// <param name="configuration">配置</param>
    /// <param name="jwtService">JWT服务</param>
    public HzgAccountController(AccountDbContext accountContext,
                             IConfiguration configuration,
                             IJwtService jwtService,
                             ILogger<LoginViewModel> logger,
                             IUserService userService,
                             IEmailService emailService,
                             IVerifyCodeService verifyCodeService)
    {
        _accountContext = accountContext;
        _configuration = configuration;
        _jwtService = jwtService;
        _userService = userService;
        _logger = logger;
        _emailService = emailService;
        _verifyCodeService = verifyCodeService;
    }

    /// <summary>
    /// 获取注册验证码
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [Route("send-register-verify-code")]
    public async Task<string> SendRegisterVerifyCode(string email)
    {
        var result = new ResponseData()
        {
            Code = ErrorCode.Success
        };

        // 检测邮箱是否已存在
        var user = await _accountContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user != null)
        {
            result.Code = ErrorCode.User_Has_Exist;
            return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());;
        }

        // 1. 生成验证码
        // 2. 保存验证码到 redis
        // 3. 发送验证码到邮箱
        // _verifyCodeService.SendRegisterVerifyCode(email);

        return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 注册账号流程
    /// 1. 获取验证码
    /// 2. 输入注册信息及收到的验证码
    /// 3. 注册账号
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<string> Register([FromBody] RegisterModel model)
    {
        var result = new ResponseData()
        {
            Code = ErrorCode.Failed
        };

        // 检测用户是否已经注册
        var exitUser = await _accountContext.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
        if (exitUser != null)
        {
            result.Code = ErrorCode.User_Has_Exist;

            return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
        }

        // 获取本地验证码
        var localVerifyCode = RedisTool.GetStringValue(model.Email);
        if (localVerifyCode == null || localVerifyCode.Equals(model.VerifyCode) == false)
        {
            // 验证码不一致
            result.Code = ErrorCode.VerifyCode_Incorrect;

            return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
        }

        var user = new HzgUser();

        user.Name = model.Email;
        user.Email = model.Email;
        user.Password = model.Password;
    
        _accountContext.Users.Add(user);

        var count = await _accountContext.SaveChangesAsync();
        if (count != 1)
        {

        }

        result.Code = ErrorCode.Registered_Successfully;

        return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [Route("[action]")]
    public async Task<string> Login([FromBody] LoginViewModel model)
    {
        var result = new ResponseData() {
            Code = ErrorCode.Success
        };

        var user = await _accountContext.Users.SingleOrDefaultAsync(u => u.Name == model.UserName);
        if (user == null)
        {
            result.Code = ErrorCode.User_Not_Exist;

            return JsonSerializerTool.SerializeDefault(result);
        }
        var password = MD5Tool.Encrypt(model.Password, user.Salt);
        if (password != user.Password)
        {
            result.Code = ErrorCode.Password_Not_Correct;

            return JsonSerializerTool.SerializeDefault(result);
        }

        if (user != null)
        {
            var userDto = new UserDto();

            userDto.UserName = user.Name;
            userDto.Id = user.Id.ToString();
            userDto.Email = user.Email;

            var jwtToken = _jwtService.GetnerateJWTToken(userDto);

            var menusToReturn = await MenuTool.GetUserPermissionMenus(_accountContext, user.Id);

            result.Data = new
            {
                token = jwtToken,
                menuData = menusToReturn,
                Brand = user.Brand
            };

            return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
        }

        result.Code = ErrorCode.Account_Or_Password_Not_Correct;

        return JsonSerializer.Serialize(result, JsonSerializerTool.DefaultOptions());
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("[action]")]
    public async Task<ResponseData<string>> LogOut()
    {
        return await this._userService.Logout();
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("user/info")]
    public async Task<IActionResult> UserInfo(string token)
    {
        // 获取用户信息
        Guid? userId = _jwtService.GetUserId(token);
        var user = await _accountContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            // return new OkObjectResult(new { code = 20000, data = new { roles = user.Role.Split(",") , name = user.Name, group = user.Group, avatar = "", introduction = "introduction" } });
        }

        return new BadRequestResult();
    }

    /// <summary>
    /// 获取指定分组的用户,如果分组名为空,则获取所有
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("groupusers")]
    public async Task<string> GetGroupUsers(string group)
    {
        var users = new List<HzgUser>();
        if (group != null)
        {
            var groupModel = await _accountContext.Groups.SingleOrDefaultAsync(g => g.Name == group);
            if (groupModel != null)
            {
                users = groupModel.UserGroups.Select((ug, i) => ug.User).ToList();
            }
        }
        else
        {
            users = await _accountContext.Users.AsNoTracking().ToListAsync();
        }

        var response = new { code = ErrorCode.Success, data = users };

        return JsonSerializer.Serialize(response, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });
    }

    /// <summary>
    /// 获取所有分组
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("getallgroups")]
    public async Task<string> GetAllGroups()
    {
        var allGroups = await _accountContext.Groups.AsNoTracking().OrderBy(g => g.Name).ToListAsync();

        var response = new { code = ErrorCode.Success, data = allGroups };

        return JsonSerializer.Serialize(response);
    }

    /// <summary>
    /// 获取用户目录树数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get-user-treedata")]
    public async Task<string> GetUserTreeData()
    {
        // 数据包含
        // 1. 分组中的用户
        // 2. 无分组的用户
        // 格式 [{ label: '', children: [ { label: '', children: [] } ] }]
        var userTreeData = new List<UserTreeNode>();
        var groups = await _accountContext.Groups.AsNoTracking().OrderBy(g => g.Name).ToListAsync();
        var users = await _accountContext.Users.AsNoTracking().OrderBy(u => u.Name).ToListAsync();
        var roles = await _accountContext.Roles.AsNoTracking().ToListAsync();
        var groupsUsers = await _accountContext.UserGroups.AsNoTracking().ToListAsync();
        var userRoles = await _accountContext.UserRoles.AsNoTracking().ToListAsync();

        foreach(var m in groups)
        {
            var nodeModel = new UserTreeNode();

            nodeModel.NodeKey = m.Id.ToString();
            nodeModel.Id = m.Id;
            nodeModel.ParentMenuId = null;
            nodeModel.IsLeaf = false;
            nodeModel.Disabled = true;
            nodeModel.IsGroup = true;

            // 获取分组成员
            var childrenUserTreeData = new List<UserTreeNode>();
            foreach(var gu in groupsUsers.Where(gu => gu.GroupId == m.Id))
            {
                var user = users.SingleOrDefault(u => u.Id == gu.UserId);
                if (user == null)
                {
                    continue;
                }

                var userNodeModel = GenerateUserTreeNode(user, m.Id.ToString(), userRoles, roles);

                childrenUserTreeData.Add(userNodeModel);
            }

            nodeModel.Children = childrenUserTreeData.ToArray();
            nodeModel.Label = m.Name + " (" + nodeModel.Children.Length.ToString() + "人)";
            nodeModel.LabelValue = m.Name;

            userTreeData.Add(nodeModel);
        }

        // 添加无分组用户
        foreach(var u in users)
        {
            if (groupsUsers.Where(gu => gu.UserId == u.Id).ToArray().Length > 0)
            {
                continue;
            }

            var userNodeModel = GenerateUserTreeNode(u, "", userRoles, roles);

            userTreeData.Add(userNodeModel);
        }

        var responseData = new ResponseData()
        {
            Code = ErrorCode.Success,
            Data = userTreeData
        };

        return JsonSerializerTool.SerializeDefault(responseData);
    }

    /// <summary>
    /// 生成用户节点
    /// </summary>
    /// <returns></returns>
    private UserTreeNode GenerateUserTreeNode(HzgUser u, string groupId, List<HzgUserRole> userRoles, List<HzgRole> roles)
    {
        var userNodeModel = new UserTreeNode();
        
        userNodeModel.NodeKey = groupId + "-" + u.Id.ToString();
        userNodeModel.Id = u.Id;
        userNodeModel.ParentMenuId = null;
        userNodeModel.LabelValue = u.Name;
        userNodeModel.IsLeaf = true;
        userNodeModel.Disabled = false;

        // 添加角色
        string formatRoles = "";
        foreach(var ur in userRoles.Where(ur => ur.UserId == u.Id))
        {
            var currentRole = roles.SingleOrDefault(r => r.Id == ur.RoleId);
            if (currentRole == null)
            {
                continue;
            }

            formatRoles = formatRoles + currentRole.Name + ",";
        }

        userNodeModel.Label = u.Name +  (formatRoles == "" ? "" : " (" + formatRoles.TrimEnd(',') + ")");

        return userNodeModel;
    }

    public class UserTreeNode
    {
        public string NodeKey { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentMenuId { get; set; }
        public string Label { get; set; }
        public string LabelValue { get; set; }
        public UserTreeNode[] Children { get; set; }
        public bool IsLeaf { get; set; }
        public bool Disabled { get; set; }

        /// <summary>
        /// 是否是分组，如果是无分组的用户，则设置为 False
        /// </summary>
        /// <value></value>
        public bool IsGroup { get; set; }
    }

    /// <summary>
    /// 登录用户信息
    /// </summary>
    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// 前端显示的用户信息
    /// </summary>
    public class UserInfoDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }            
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
    }
}