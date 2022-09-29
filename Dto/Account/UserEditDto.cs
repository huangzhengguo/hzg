namespace Hzg.Dto;

/// <summary>
/// 创建用户信息
/// </summary>
public class UserEditDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string[] GroupIds { get; set; }
    public string[] RoleIds { get; set; }
    public string Password { get; set; }
}