using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hzg.Controllers;

/// <summary>
/// API 控制器基类
/// </summary>
[Authorize]
[ApiController]
public class BaseController : ControllerBase
{

}