namespace Hzg.Dto;

/// <summary>
/// 创建用户信息
/// </summary>
public class UserInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public Guid[] GroupIds { get; set; }
    public Guid[] RoleIds { get; set; }
}