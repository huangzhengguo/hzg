using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Dapper;
using Hzg.Models;
using Hzg.Tool;
using Hzg.Dto;
using Hzg.Services;

namespace Hzg.Controllers;

/// <summary>
/// 企业信息
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]/")]
public class EnterpriseController : BaseController
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    public EnterpriseController(IConfiguration configuration, IUserService userService)
    {
        this._configuration = configuration;
        this._userService = userService;
    }

    /// <summary>
    /// 获取所有企业信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get_all")]
    public async Task<string> GetAll()
    {
        var responseData = new ResponseData()
        {
            Code = Const.ErrorCode.ErrorCode_Success,
            Message = "成功"
        };

        var connect = new MySqlConnector.MySqlConnection(_configuration.GetConnectionString("IotDbConnection"));

        var data = await connect.QueryAsync<EnterpriseDto>(@"select id, corp_id AS CorpId, name, mail_host AS MailHost, mail_port AS MailPort," +
                                                           @"mail_encoding AS MailEncoding, mail_protocol AS MailProtocol," +
                                                           @"mail_username AS MailUsername,mail_password AS MailPassword," +
                                                           @"mail_properties_ssl_enable AS MailPropertiesSslEnable, mail_registration_subject AS MailRegistrationSubject, mail_registration_content AS MailRegistrationContent," +
                                                           @"mail_reset_subject AS MailResetSubject, mail_reset_content AS MailResetContent from enterprise");

        responseData.Data = data;

        return JsonSerializerTool.SerializeDefault(responseData);
    }

    /// <summary>
    /// 获取所有企业信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get/{id}")]
    public async Task<string> GetDetail([FromRoute] string id)
    {
        var responseData = new ResponseData()
        {
            Code = Const.ErrorCode.ErrorCode_Success,
            Message = "success"
        };

        var data = await GetEnterpriseDto(id);
        if (data != null)
        {
            responseData.Data = data;
        }
        else
        {
            responseData.Code = Const.ErrorCode.ErrorCode_Failed;
            responseData.Message = "Corp Not Exist!";

            return JsonSerializerTool.SerializeDefault(responseData);
        }

        return JsonSerializerTool.SerializeDefault(responseData);
    }

    /// <summary>
    /// 新建
    /// </summary>
    /// <param name="enterprise"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<string> Create([FromBody] EnterpriseDto enterpriseDto)
    {
        var responseData = new ResponseData()
        {
            Code = Const.ErrorCode.ErrorCode_Success,
            Message = LocalizerTool.Value("success")
        };

        var connect = new MySqlConnector.MySqlConnection(_configuration.GetConnectionString("IotDbConnection"));

        var creator = await _userService.GetCurrentUserId();
        var enterpriseId = Guid.NewGuid().ToString();
        var corpId = enterpriseId;
        if (String.IsNullOrWhiteSpace(enterpriseDto.CorpId) == false)
        {
            corpId = enterpriseDto.CorpId;
        }
        var count = await connect.ExecuteAsync(@"insert Enterprise(id, corp_id, name, contacts_user, contacts_phone, sort, create_time, creator,
                                               mail_host, mail_port, mail_encoding, mail_protocol, mail_username, mail_password,
                                               mail_properties_ssl_enable, mail_registration_subject, mail_registration_content,
                                               mail_reset_subject, mail_reset_content)
                                  values('" + enterpriseId + @"','" + enterpriseId + @"', @Name, '', '', @Sort, '" + DateTime.Now + @"', '" + creator.ToString() + 
                                  @"', @MailHost, @MailPort, @MailEncoding, @MailProtocol, @MailUsername, @MailPassword, 
                                  @MailPropertiesSslEnable, @MailRegistrationSubject, @MailRegistrationContent, @MailResetSubject,
                                  @MailResetContent)", enterpriseDto);
        if (count != 1)
        {
            responseData.Code = Const.ErrorCode.ErrorCode_Failed;
            responseData.Message = LocalizerTool.Value("failed");

            return JsonSerializerTool.SerializeDefault(responseData);
        }

        var dto = await GetEnterpriseDto(enterpriseId);

        responseData.Data = dto;
        responseData.ShowMsg = true;

        return JsonSerializerTool.SerializeDefault(responseData);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="enterprise"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("update")]
    public async Task<string> update([FromBody] EnterpriseDto enterpriseDto)
    {
        var responseData = new ResponseData()
        {
            Code = Const.ErrorCode.ErrorCode_Success,
            Message = LocalizerTool.Value("success")
        };

        var connect = new MySqlConnector.MySqlConnection(_configuration.GetConnectionString("IotDbConnection"));

        var count = await connect.ExecuteAsync(@"update Enterprise set corp_id = @CorpId, name = @Name, mail_host= @MailHost, mail_port=@MailPort,
                                               mail_encoding = @MailEncoding, mail_protocol = @MailProtocol, mail_username = @MailUsername,
                                               mail_password = @MailPassword, mail_properties_ssl_enable = @MailPropertiesSslEnable,
                                               mail_registration_subject = @MailRegistrationSubject,
                                               mail_registration_content = @MailRegistrationContent,
                                               mail_reset_subject = @MailResetSubject,
                                               mail_reset_content = @MailResetContent where id = @Id", enterpriseDto);
        if (count != 1)
        {
            responseData.Code = Const.ErrorCode.ErrorCode_Failed;
            responseData.Message = LocalizerTool.Value("failed");

            return JsonSerializerTool.SerializeDefault(responseData);
        }

        var data = await GetEnterpriseDto(enterpriseDto.Id.ToString());

        responseData.ShowMsg = true;
        responseData.Data = data;

        return JsonSerializerTool.SerializeDefault(responseData);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="enterprise"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<string> delete([FromRoute] string id)
    {
        var responseData = new ResponseData()
        {
            Code = Const.ErrorCode.ErrorCode_Success,
            Message = LocalizerTool.Value("success")
        };

        var connect = new MySqlConnector.MySqlConnection(_configuration.GetConnectionString("IotDbConnection"));

        var count = await connect.ExecuteAsync(@"delete from Enterprise where id = '" + id + "'");
        if (count != 1)
        {
            responseData.Code = Const.ErrorCode.ErrorCode_Failed;
            responseData.Message = LocalizerTool.Value("notExist");

            return JsonSerializerTool.SerializeDefault(responseData);
        }

        responseData.ShowMsg = true;
        responseData.Message = LocalizerTool.Value("deleteSuccess");

        return JsonSerializerTool.SerializeDefault(responseData);
    }

    /// <summary>
    /// 获取指定企业信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<EnterpriseDto> GetEnterpriseDto(string id)
    {
        var connect = new MySqlConnector.MySqlConnection(_configuration.GetConnectionString("IotDbConnection"));
        var data = await connect.QueryAsync<EnterpriseDto>(@"select id, corp_id AS CorpId, name, mail_host AS MailHost, mail_port AS MailPort," +
                                                           @"mail_encoding AS MailEncoding, mail_protocol AS MailProtocol," +
                                                           @"mail_username AS MailUsername,mail_password AS MailPassword," +
                                                           @"mail_properties_ssl_enable AS MailPropertiesSslEnable, mail_registration_subject AS MailRegistrationSubject, mail_registration_content AS MailRegistrationContent," +
                                                           @"mail_reset_subject AS MailResetSubject, mail_reset_content AS MailResetContent from enterprise where id = @Id", new { Id = id });
        if (data.Count() > 0)
        {
            return data.First();
        }

        return null;
    }
}